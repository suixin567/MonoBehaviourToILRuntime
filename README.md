**简介**

MonoBehaviourToILRuntime是针对Unity3D项目的热更方案，代码简洁，接入方便。

此框架的作用是通过最小的修改让以传统MonoBehaviour写法的老项目快速获得完善的热更新能力。

当然新项目也可以用，可以继续使用熟悉的MonoBehaviour写法，却具备热更能力。

核心原理: 运行时将Mono脚本自动替换为热更脚本）

**方案结构**

双热更，既包括在Android平台方便好用的Mono热更，还有更适合在IOS平台使用的ILRuntime热更。

**开发环境**

Unity3D: 2019.4.15f1c1

ILRuntime: 1.65

Ab包工具: XAsset: 4.0

VS: 2019

**使用步骤**

1、启动Assets/Model/Init.unity场景

2、启动后，参考Assets/Hotfix/Script/FirstScene.cs文件

**关于本框架的其他插件**

ProtoBuf 后续更新

**贡献成员**

[3块给你买麻薯](https://github.com/suixin567)

[YinHuaNan](https://github.com/YinHuaNan)

[404](https://github.com/404Lccxy)

**更多项目**

- [ET](https://github.com/egametang/ET) Unity3D Client And C# Server Framework
- [ILRuntime](https://github.com/Ourpalm/ILRuntime) c#虚拟机
- [XAsset](https://github.com/xasset/xasset) 精简、高效、安全的Unity资源管理方案。

