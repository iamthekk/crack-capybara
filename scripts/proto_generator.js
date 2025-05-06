const fs = require('fs-extra');
const path = require('path');
const glob = require('glob');


// 检查命令行参数
if (process.argv.length < 4) {
    console.log('用法: node proto_generator.js <json目录路径> <输出目录路径>');
    process.exit(1);
}

// 配置路径
const jsonDir = process.argv[2]
const outputDir = process.argv[3]


// 确保输出目录存在
fs.ensureDirSync(outputDir);

/**
 * 根据属性名生成字段名
 * @param {string} propertyName 属性名
 * @returns {string} 字段名
 */
function getFieldNameFromProperty(propertyName) {
    // 将属性名转换为字段名（首字母小写 + 下划线）
    return propertyName.charAt(0).toLowerCase() + propertyName.slice(1) + '_';
}

/**
 * 检查类型是否是只读集合类型
 * @param {string} typeName 类型名
 * @returns {boolean} 是否是只读集合类型
 */
function isReadOnlyCollectionType(typeName) {
    return typeName.startsWith('RepeatedField<') || typeName.startsWith('MapField<');
}

/**
 * 从泛型类型名提取泛型参数
 * @param {string} genericTypeName 泛型类型名
 * @returns {string} 泛型参数
 */
function extractGenericType(genericTypeName) {
    const match = /<([^>]+)>/.exec(genericTypeName);
    if (match) {
        return match[1];
    }
    return 'object'; // 默认值
}

/**
 * 根据类型确定FieldCodec的方法名
 * @param {string} typeName 类型名
 * @returns {string} FieldCodec方法名
 */
function getFieldCodecMethod(typeName) {
    const lowerType = typeName.toLowerCase();

    if (lowerType === 'int' || lowerType === 'int32') return 'Int32';
    if (lowerType === 'long' || lowerType === 'int64') return 'Int64';
    if (lowerType === 'uint' || lowerType === 'uint32') return 'UInt32';
    if (lowerType === 'ulong' || lowerType === 'uint64') return 'UInt64';
    if (lowerType === 'bool') return 'Bool';
    if (lowerType === 'string') return 'String';
    if (lowerType === 'float') return 'Float';
    if (lowerType === 'double') return 'Double';
    if (lowerType === 'byte[]') return 'Bytes';

    return 'Message';
}

/**
 * 添加IMessage接口必需的方法声明（占位实现）
 * @param {Array<string>} lines 代码行数组
 * @param {Object} classInfo 类信息
 */
function addMessageMethods(lines, classInfo) {
    // 添加WriteTo方法
    lines.push('        [DebuggerNonUserCode]');
    lines.push('        public void WriteTo(CodedOutputStream output)');
    lines.push('        {');
    lines.push('            // 此处为占位符，实际实现需要根据字段类型和序号来生成');
    lines.push('        }');
    lines.push('');

    // 添加CalculateSize方法
    lines.push('        [DebuggerNonUserCode]');
    lines.push('        public int CalculateSize()');
    lines.push('        {');
    lines.push('            // 此处为占位符，实际实现需要根据字段类型和序号来生成');
    lines.push('            return 0;');
    lines.push('        }');
    lines.push('');

    // 添加MergeFrom方法
    lines.push('        [DebuggerNonUserCode]');
    lines.push('        public void MergeFrom(CodedInputStream input)');
    lines.push('        {');
    lines.push('            // 此处为占位符，实际实现需要根据字段类型和序号来生成');
    lines.push('        }');
    lines.push('');
}

/**
 * 添加私有字段部分
 * @param {Array<string>} lines 代码行数组
 * @param {Object} classInfo 类信息
 */
function addPrivateFields(lines, classInfo) {
    // 添加_parser静态字段
    lines.push(`        private static readonly MessageParser<${classInfo.ClassName}> _parser = new MessageParser<${classInfo.ClassName}>(() => new ${classInfo.ClassName}());`);
    lines.push('');

    // 为每个属性添加字段和常量
    let fieldNumber = 1;
    for (const prop of classInfo.Properties) {
        // 添加字段编号常量
        lines.push(`        public const int ${prop.Name}FieldNumber = ${fieldNumber};`);
        lines.push('');

        // 添加私有字段
        const fieldName = getFieldNameFromProperty(prop.Name);

        // 处理特殊集合类型
        if (prop.Type.startsWith('RepeatedField<')) {
            const elementType = extractGenericType(prop.Type);
            lines.push(`        private static readonly FieldCodec<${elementType}> _repeated_${fieldName}_codec = FieldCodec.For${getFieldCodecMethod(elementType)}(${fieldNumber * 8}U);`);
            lines.push('');
            lines.push(`        private readonly ${prop.Type} ${fieldName} = new ${prop.Type}();`);
        } else if (prop.Type === 'string') {
            lines.push(`        private string ${fieldName} = "";`);
        } else {
            lines.push(`        private ${prop.Type} ${fieldName}_;`);
        }

        fieldNumber++;
    }
}

/**
 * 生成C#代码
 * @param {Object} classInfo 类信息
 * @returns {string} 生成的C#代码
 */
function generateCode(classInfo) {
    const lines = [];

    // 添加using语句
    for (const usingStatement of classInfo.Usings) {
        lines.push(`using ${usingStatement};`);
    }
    lines.push('');

    // 添加命名空间和类
    lines.push(`namespace ${classInfo.Namespace}`);
    lines.push('{');
    lines.push(`    public sealed class ${classInfo.ClassName} : IMessage`);
    lines.push('    {');

    // 添加属性
    for (const prop of classInfo.Properties) {
        // 处理属性类型，确保复杂泛型类型格式正确
        lines.push(`        public ${prop.Type.startsWith('RepeatedField') ? prop.Type.replace('RepeatedField', 'List') : prop.Type} ${prop.Name};`);
        lines.push('');
    }

    lines.push('    }');
    lines.push('}');

    return lines.join('\n');
}

/**
 * 处理单个JSON文件
 * @param {string} jsonFilePath JSON文件路径
 * @param {string} outputDir 输出目录
 */
function processFile(jsonFilePath, outputDir) {
    try {
        // 读取JSON文件
        const jsonContent = fs.readFileSync(jsonFilePath, 'utf8');
        let classInfo;

        try {
            classInfo = JSON.parse(jsonContent);
        } catch (parseError) {
            console.error(`无法解析JSON文件 ${jsonFilePath}: ${parseError.message}`);
            return;
        }

        if (!classInfo || !classInfo.Properties || classInfo.Properties.length === 0) {
            console.warn(`警告：JSON文件 ${jsonFilePath} 不包含属性信息`);
            return;
        }

        const fileName = path.basename(jsonFilePath, '.json');
        const outputFilePath = path.join(outputDir, `${fileName}.cs`);

        // 生成C#代码
        const code = generateCode(classInfo);

        // 写入文件
        fs.writeFileSync(outputFilePath, code);
        console.log(`已生成：${outputFilePath} (属性数量: ${classInfo.Properties.length})`);
    } catch (error) {
        console.error(`处理文件 ${jsonFilePath} 出错：${error.message}`);
    }
}

/**
 * 处理目录
 * @param {string} sourceDir 源目录
 * @param {string} outputDir 输出目录
 */
function processDirectory(sourceDir, outputDir) {
    try {
        // 确保输出目录存在
        fs.ensureDirSync(outputDir);

        // 获取该目录下的所有JSON文件
        const files = glob.sync('*.json', { cwd: sourceDir, absolute: true });

        if (files.length === 0) {
            console.log(`目录 ${sourceDir} 中没有找到JSON文件`);
        } else {
            console.log(`在目录 ${sourceDir} 中找到 ${files.length} 个JSON文件`);
        }

        // 处理每个文件
        files.forEach(file => {
            processFile(file, outputDir);
        });

        // 递归处理子目录
        const dirs = fs.readdirSync(sourceDir, { withFileTypes: true })
            .filter(dirent => dirent.isDirectory())
            .map(dirent => dirent.name);

        for (const dir of dirs) {
            const subSourceDir = path.join(sourceDir, dir);
            const subOutputDir = path.join(outputDir, dir);
            processDirectory(subSourceDir, subOutputDir);
        }
    } catch (error) {
        console.error(`处理目录 ${sourceDir} 时出错：${error.message}`);
    }
}

// 开始处理
console.log('开始根据JSON生成Proto类文件...');
processDirectory(jsonDir, outputDir);
console.log('生成完成！'); 