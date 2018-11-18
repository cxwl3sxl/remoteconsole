# Remote Console Log
远程日志工具
用于将写入console的日志信息发送到远程，然后在web界面上进行查看
主要使用方法为：
启动程序，右键状态栏中的图标，点击复制，然后得到如下内容
> <script src='http://localhost:8081/Scripts/client.js'></script>
将上述脚本拷贝到需要调试的页面中，例如如下代码

`<!DOCTYPE html>`

`<html lang="en" xmlns="http://www.w3.org/1999/xhtml">` 

`<head>` 

`    <meta charset="utf-8" />`

`    <title></title>`

`    <script src='http://localhost:8081/Scripts/client.js'></script>`

`</head>`

`<body>`

`    <button type="button" onclick="console.log('log')">log</button>`

`    <button type="button" onclick="console.info('info')">info</button>`

`    <button type="button" onclick="console.warn('warn')">warn</button>`

`    <button type="button" onclick="console.error('error')">error</button>`

`</body>`

`</html>`

打开页面之后，点击所有按钮，产生的日志将在远程界面上显示出来。

如下图：
![](https://github.com/cxwl3sxl/remoteconsole/blob/master/logview.PNG)
本工具主要基于SingalR实现

版权没有，欢迎拷贝
