# 🌈 RAINBOW FART FOR VISUAL STUDIO
 
 
 ## 概述

**Rainbow Fart** 是一个在你编程时持续夸你写的牛逼的扩展，可以根据代码关键字播放贴近代码意义的真人语音。

## 快速上手


(测试环境VS2017、VS2019、VS2022，未测其他版本， 应该可以支持VS2015 - VS2022所有版本)

1. 安装扩展<br />
  方法1：[从release下载VSIX直接安装](https://github.com/gameguo/visualstudio-rainbow-fart/releases/tag/v2.1)<br />
  VS2022 请下载 visualstudio-rainbow-fart_2022.vsix<br />
  VS2019 及更老版本请下载 visualstudio-rainbow-fart.vsix<br /><br />
  **以下链接只有2019及其以下版本：**<br />
  方法2：[已发布VisualStudio扩展商店](https://marketplace.visualstudio.com/items?itemName=gameguo.rainbow-fart)<br />
  ~~方法3：VisualStudio 工具 >> 扩展与更新 >> 联机 >> 搜索：rainbow-fart  或  彩虹屁  (刚刚发布似乎还搜不到)~~

2. VS2017：点击菜单项 "彩虹屁" >> "测试声音播放"<br />
   VS2019/2022：点击菜单项 "扩展" >> "彩虹屁" >> "测试声音播放"

3. 打开一个代码文件，尝试输入import、using、if等关键字<br />
（关键字可通过 音频资源路径/contributes.json中自行添加，目前已添加部分C#）

4. VS2017：可在菜单项 >> "彩虹屁" >> "设置" 中指定开关与音频资源路径<br />
   VS2019/2022：可在菜单项 >> "扩展" >> "彩虹屁" >> "设置" 中指定开关与音频资源路径

## 关于编译

#### 项目最后使用VS2019与VS2022编译通过, VS2022版本扩展与VS2019及其以下版本扩展无法使用同一个vsix发布。<br /> <br />
发布时：<br />
默认使用[source.extension.vsixmanifest](https://github.com/gameguo/visualstudio-rainbow-fart/blob/main/visualstudio-rainbow-fart/source.extension.vsixmanifest)发布VS2019及其以下版本  <br />
发布VS2022, 请把 [source.extension_2022.vsixmanifest](https://github.com/gameguo/visualstudio-rainbow-fart/blob/main/visualstudio-rainbow-fart/source.extension_2022.vsixmanifest) 重命名为source.extension.vsixmanifest进行发布 <br />
 <br />

## License
基于 MIT 开源，包括所有设计资源及音频资源。由于仓库中的音频资源大部分由真人录音，并且根据 MIT 被授权人义务。在此明确：尤其的对于仓库中多媒体资源，您有（单独）标明资源作者（[@JustKowalski](https://github.com/JustKowalski) 提供。）、链接、许可的义务。

此外，插件灵感与素材来自VSCode插件作者[@SaekiRaku](https://github.com/SaekiRaku/vscode-rainbow-fart)

该项目为 VisualStudio C#语言版本的实现

<br /> <br />
<img src="https://github.com/gameguo/visualstudio-rainbow-fart/blob/main/document/vs2019.png?raw=true" alt="1" width="1000" />
