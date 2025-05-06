const fs = require('fs-extra');
const path = require('path');
const glob = require('glob');


// 检查命令行参数
if (process.argv.length < 4) {
    console.log('用法: node proto_analyzer.js <Proto目录路径> <输出目录路径>');
    process.exit(1);
}

// 配置路径
const protoDir = process.argv[2]
const outputDir = process.argv[3]



// 验证目录是否存在
if (!fs.existsSync(protoDir)) {
    console.error(`错误: Proto目录 "${protoDir}" 不存在`);
    process.exit(1);
}

console.log(`开始分析Proto目录: ${protoDir}`);
console.log(`输出目录: ${outputDir}`);


// 确保输出目录存在
fs.ensureDirSync(outputDir);

/**
 * 解析CS文件，提取类信息
 * @param {string} filePath CS文件路径
 * @returns {Object|null} 解析后的类信息对象
 */
function parseFile(filePath) {
    try {
        const fileContent = fs.readFileSync(filePath, 'utf-8');
        const lines = fileContent.split(/\r?\n/);

        const classInfo = {
            Namespace: '',
            ClassName: '',
            Usings: [],
            Properties: []
        };

        // 解析using语句
        for (const line of lines) {
            if (line.trim().startsWith('using ') && line.trim().endsWith(';')) {
                const usingMatch = line.match(/using\s+([^;]+);/);
                if (usingMatch) {
                    classInfo.Usings.push(usingMatch[1].trim());
                }
            }
        }

        // 解析命名空间
        for (const line of lines) {
            if (line.trim().startsWith('namespace ')) {
                const namespaceMatch = line.match(/namespace\s+([^\s{]+)/);
                if (namespaceMatch) {
                    classInfo.Namespace = namespaceMatch[1];
                    break;
                }
            }
        }

        // 解析类名
        let classFound = false;
        for (const line of lines) {
            if (!classFound && line.includes('class ') && !line.includes('@class')) {
                const classMatch = line.match(/\bclass\s+([^\s:<]+)/);
                if (classMatch) {
                    classInfo.ClassName = classMatch[1];
                    classFound = true;
                    break;
                }
            }
        }

        if (!classFound || !classInfo.ClassName) {
            return null;
        }

        // 只处理IMessage类型的类
        let isIMessageClass = false;
        for (const line of lines) {
            if (line.includes(classInfo.ClassName) && line.includes(':') && line.includes('IMessage')) {
                isIMessageClass = true;
                break;
            }
        }

        if (!isIMessageClass) {
            return null;
        }

        // 查找属性和字段
        const foundFields = new Set();

        // 首先检查MapField和RepeatedField特殊类型定义
        const fieldTypeMap = {};

        // 搜索MapField和RepeatedField类型定义
        for (const line of lines) {
            // MapField类型
            if (line.includes('MapField<')) {
                const mapFieldMatch = line.match(/MapField<([^>]+)>\s+(\w+)(?:_)?/);
                if (mapFieldMatch) {
                    const fieldTypeParams = mapFieldMatch[1].trim();
                    const fieldVarName = mapFieldMatch[2];

                    // 转换变量名为属性名 (通常是首字母大写)
                    const propName = fieldVarName.charAt(0).toUpperCase() + fieldVarName.slice(1).replace('_', '');
                    fieldTypeMap[propName] = `MapField<${fieldTypeParams}>`;
                }
            }

            // RepeatedField类型
            if (line.includes('RepeatedField<')) {
                const repeatedFieldMatch = line.match(/RepeatedField<([^>]+)>\s+(\w+)(?:_)?/);
                if (repeatedFieldMatch) {
                    const fieldTypeParam = repeatedFieldMatch[1].trim();
                    const fieldVarName = repeatedFieldMatch[2];

                    // 转换变量名为属性名
                    const propName = fieldVarName.charAt(0).toUpperCase() + fieldVarName.slice(1).replace('_', '');
                    fieldTypeMap[propName] = `RepeatedField<${fieldTypeParam}>`;
                }
            }
        }

        // 搜索常量以查找属性标识符
        const fieldNumberConstants = [];
        for (const line of lines) {
            const constFieldMatch = line.match(/public\s+const\s+int\s+(\w+)FieldNumber\s*=/);
            if (constFieldMatch) {
                const fieldName = constFieldMatch[1];
                fieldNumberConstants.push(fieldName);
            }
        }

        // 使用字段名称常量查找对应的属性类型
        for (const fieldName of fieldNumberConstants) {
            let fieldType = null;

            // 首先检查特殊类型映射中是否存在
            if (fieldTypeMap[fieldName]) {
                fieldType = fieldTypeMap[fieldName];
            } else {
                // 查找属性定义
                for (const line of lines) {
                    // 检查属性类型
                    if (line.includes(`public `) && line.includes(` ${fieldName}`) && !line.includes('FieldNumber')) {
                        const match = line.match(/public\s+([\w\.<>, ]+?)\s+${fieldName}\s*[\{;]/);
                        if (match) {
                            fieldType = match[1].trim();
                            break;
                        }
                    }
                }

                // 如果没有找到属性类型，查找字段定义
                if (!fieldType) {
                    const fieldVarName = fieldName.charAt(0).toLowerCase() + fieldName.slice(1) + '_';
                    for (const line of lines) {
                        if (line.includes(`private `) && line.includes(fieldVarName)) {
                            // 检查RepeatedField类型
                            let repeatedMatch = line.match(/private\s+(?:readonly\s+)?(RepeatedField<[^>]+>)/);
                            if (repeatedMatch) {
                                fieldType = repeatedMatch[1];
                                break;
                            }

                            // 检查MapField类型
                            let mapMatch = line.match(/private\s+(?:readonly\s+)?(MapField<[^>]+>)/);
                            if (mapMatch) {
                                fieldType = mapMatch[1];
                                break;
                            }

                            // 检查一般类型
                            let typeMatch = line.match(/private\s+(?:readonly\s+)?([^\s]+)\s+/);
                            if (typeMatch && !typeMatch[1].includes('static')) {
                                fieldType = typeMatch[1].replace('_', '');
                                break;
                            }
                        }
                    }
                }
            }

            if (fieldType) {
                // 清理类型，移除多余空格
                fieldType = fieldType.replace(/\s+/g, ' ').trim();

                // 添加属性到列表
                classInfo.Properties.push({
                    Name: fieldName,
                    Type: fieldType
                });

                foundFields.add(fieldName);
            }
        }

        // 直接查找带[DebuggerNonUserCode]标记的属性
        for (let i = 0; i < lines.length; i++) {
            if (lines[i].includes('[DebuggerNonUserCode]') && i + 1 < lines.length) {
                const nextLine = lines[i + 1];
                if (nextLine.includes('public ') && !nextLine.includes('(')) {
                    const propMatch = nextLine.match(/public\s+([\w\.<>, ]+?)\s+(\w+)\s*\{/);
                    if (propMatch) {
                        const propName = propMatch[2];
                        const propType = propMatch[1].trim();

                        // 查找是否存在于特殊类型映射中
                        const mappedType = fieldTypeMap[propName];

                        // 避免重复添加
                        if (!foundFields.has(propName)) {
                            classInfo.Properties.push({
                                Name: propName,
                                Type: mappedType || propType
                            });
                            foundFields.add(propName);
                        }
                    }
                }
            }
        }

        // 直接查找公共字段
        for (const line of lines) {
            if (line.includes('public ') && line.includes(';') && !line.includes('const ')) {
                const fieldMatch = line.match(/public\s+([\w\.<>, ]+?)\s+(\w+)\s*;/);
                if (fieldMatch) {
                    const fieldName = fieldMatch[2];
                    const fieldType = fieldMatch[1].trim();

                    // 查找是否存在于特殊类型映射中
                    const mappedType = fieldTypeMap[fieldName];

                    // 避免重复添加
                    if (!foundFields.has(fieldName)) {
                        classInfo.Properties.push({
                            Name: fieldName,
                            Type: mappedType || fieldType
                        });
                        foundFields.add(fieldName);
                    }
                }
            }
        }

        // 单独处理复杂的MapField类型
        for (let i = 0; i < lines.length; i++) {
            if (lines[i].includes("MapField<") && lines[i].includes(" _map_") && i > 0) {
                // 向上查找FieldNumber常量
                const fieldNumberLine = lines[i - 1];
                const fieldMatch = fieldNumberLine.match(/public\s+const\s+int\s+(\w+)FieldNumber/);
                if (fieldMatch) {
                    const fieldName = fieldMatch[1];

                    // 提取MapField类型
                    const mapTypeMatch = lines[i].match(/MapField<([^>]+)>/);
                    if (mapTypeMatch && !foundFields.has(fieldName)) {
                        const fieldType = `MapField<${mapTypeMatch[1]}>`;
                        classInfo.Properties.push({
                            Name: fieldName,
                            Type: fieldType
                        });
                        foundFields.add(fieldName);
                    }
                }
            }
        }

        return classInfo;
    } catch (error) {
        console.error(`解析文件 ${filePath} 时出错: ${error.message}`);
        return null;
    }
}

/**
 * 处理单个文件
 * @param {string} filePath 文件路径
 * @param {string} basePath 基础路径
 */
function processFile(filePath, basePath) {
    try {
        const fileName = path.basename(filePath, '.cs');

        // 获取相对路径，保持目录结构
        const relativePath = path.relative(basePath, filePath);
        const outputPath = path.join(outputDir, path.dirname(relativePath));

        // 确保输出目录存在
        fs.ensureDirSync(outputPath);

        const outputFilePath = path.join(outputPath, `${fileName}.json`);

        // 解析文件
        const classInfo = parseFile(filePath);

        if (classInfo && classInfo.Properties.length > 0) {
            // 将结果序列化为JSON
            fs.writeJsonSync(outputFilePath, classInfo, { spaces: 2 });
            console.log(`已生成: ${outputFilePath} (属性数量: ${classInfo.Properties.length})`);
        } else if (classInfo) {
            console.log(`跳过: ${filePath} (未找到属性)`);
        } else {
            console.log(`跳过: ${filePath} (解析失败)`);
        }
    } catch (error) {
        console.error(`处理文件 ${filePath} 出错: ${error.message}`);
    }
}

/**
 * 处理目录
 * @param {string} sourceDir 源目录
 */
function processDirectory(sourceDir) {
    // 获取所有.cs文件
    const files = glob.sync('**/*.cs', { cwd: sourceDir, absolute: true });
    console.log(`找到 ${files.length} 个CS文件`);

    // 处理每个文件
    files.forEach(file => {
        processFile(file, sourceDir);
    });
}

// 开始处理
console.log('开始分析Proto目录...');
processDirectory(protoDir);
console.log('分析完成！'); 