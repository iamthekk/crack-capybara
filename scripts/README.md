# Proto文件转换工具 (Node.js版本)

本工具用于分析和生成Proto目录下的Protocol Buffers类文件。工具包含两个主要组件：

1. **proto_analyzer.js** - 分析Proto目录下的所有C#类文件，提取其公共属性、命名空间、类名和using语句，并输出为JSON文件
2. **proto_generator.js** - 根据JSON文件重新生成Protocol Buffers类文件

## 目录结构

```
Root/
  ├── Proto/               # 原始Protocol Buffers类文件
  ├── proto_json/          # 分析后生成的JSON文件
  ├── proto_parsed/        # 根据JSON重新生成的类文件
  └── scripts/             # 工具脚本
      ├── proto_analyzer.js    # 分析工具源码
      ├── proto_generator.js   # 生成工具源码
      ├── package.json         # Node.js项目配置和依赖
      └── README.md            # 工具使用说明
```

## 安装依赖

首先需要安装Node.js环境，然后运行以下命令安装必要的依赖：

```
cd scripts
npm install
```

这将安装以下依赖包：
- fs-extra：文件操作的扩展库
- glob：文件匹配模式的库

## 使用方法

在安装依赖后，可以使用npm脚本来运行工具：

1. 分析Proto文件并生成JSON：
   ```
   npm run analyze
   ```

2. 根据JSON文件重新生成类文件：
   ```
   npm run generate
   ```

3. 或者一次性执行完整流程：
   ```
   npm start
   ```

## 工具说明

### proto_analyzer.js

此工具会递归处理Proto目录下的所有.cs文件，分析每个类的以下信息：

- 命名空间
- 类名
- 所有using语句
- 公开属性（非静态）的名称和类型

分析结果将被保存为JSON文件，保持原始目录结构。

### proto_generator.js

此工具会读取proto_json目录中的JSON文件，重新生成对应的C#类文件：

- 保持原始命名空间、类名和using语句
- 重新生成公开属性
- 生成必要的接口方法（WriteTo、CalculateSize、MergeFrom等）
- 生成私有字段

重新生成的类文件将保存在proto_parsed目录中，保持原始目录结构。

## 技术特点

- 使用Node.js实现，代码更简洁且易于理解
- 使用正则表达式解析C#代码结构
- 支持递归处理嵌套目录，保持原始目录结构
- 使用Promise和异步操作提高性能

## 注意事项

- 工具需要Node.js环境（建议使用v12.0.0或更高版本）
- 重新生成的类文件中，部分方法（如WriteTo、CalculateSize、MergeFrom）只包含占位符实现
- 工具主要关注类的结构信息，不会保留原始代码中的注释和格式 