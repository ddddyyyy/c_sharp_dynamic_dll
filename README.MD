# 管理端

>　动态加载DLL的实现在[Proxy.cs](https://github.com/ddddyyyy/c_sharp_dynamic_dll/blob/main/Manager/Proxy.cs)文件，其它关于Web Service 的代码可忽略

>  项目添加了Arcgis的包，运行主机需要配置Arcgis环境，如果无需调用Arcgis相关DLL，可自行去除引用

>  建议使用Arcgis组件的DLL在内部初始化类里面进行Arcgis的注册，并且对使用的资源进行释放，以免造成程序运行时不必要的错误

## 引用的第三方框架（使用VS的Nuget包管理器进行安装）

- NLog（ NLog + NLog.Config 两个包）：日志管理框架，日志以CSV格式保存，在程序运行目录下的Log文件夹里面，按日期分割文件，可以使用文本编辑器打开。

## 功能

- 通过前端输入DLL名和要调用的函数的参数来进行对不同DLL的动态调用

## 实现

- 采用C#反射实现DLL的动态加载和调用，通过对Proxy.cs文件进行扩展，可以实现对不同方法的调用，~~写了**invokeTestMethod**函数充当参考~~，测试方法已简化删除，详细看历史记录。

- 项目动态加载的DLL的根路径为**运行的exe的目录/dynamic_dll**，可以根据部署情况自行更改，在Proxy.cs文件中的DLL_ROOT_PATH

- 采用WCF进行Web服务的开发，~~**WCF寄宿在UI线程**。~~，实践证明寄宿UI线程会导致WCF服务失去并发性，退化成单线程服务，只能通过在多线程下新建UI线程的形式来调用使用到UI组件的DLL，现在默认是所有的DLL调用都在UI线程内进行，即新建一个**Window Form窗体**之后再对DLL进行调用，后续优化可以对接口添加参数进行区分调用

- 返回的静态页面，即管理页面，在电脑中的路径为**运行的exe的目录/WebFront**

## DLL的编写

- VS 创建新项目-选择类库（.net framework）- 创建Interface类 - 创建约定好的函数 （可参考本项目的DllTest和ClassLibraryTest两个项目，两个项目实现均一样，仅仅是创建新项目的时候选择的项目类型不同）

- DLL需要采用x86编译，如果要引用Arcgis的话

## 项目运行

- 以管理员身份运行exe文件，启动时间可能会有一点久（1分钟内）
- 访问地址进入网页：http://127.0.0.1:7788/common/index/{网页名，如Subsidence}

## Manager项目目录说明

- Model：Web消息实体
- Service：Web服务
- Config：里面为跨域配置实现
- Exception：里面为Web服务的异常拦截处理器实现
- Util：通用的工具类
