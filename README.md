# Stable-diffusion-webui Bot

## 介绍
适用于[Stable-diffusion-webui](https://github.com/AUTOMATIC1111/stable-diffusion-webui)的QQBot

## 指令
1. `#作画`+prompt：调用txt2img
2. `#转图`+prompt后加图片：调用img2img(未实现)

## 配置
在配置文件(Config.json)里可以看到插件所能配置的所有键，下面是详细介绍：
|键|默认值|含义|
| :----| :---- | :---- |
|R18|false||
|WhiteMode|false|群白名单模式, true则只允许白名单内的群调用, false则只不允许黑名单的群调用|
|Admin| |管理员QQ列表|
|WhiteList| |群白名单|
|BlackList| |群黑名单|
|EnableTimespan| |允许调用的时间段|
|MaxPersonQuota|10|个人每日额度|
|MaxGroupQuota|50|群组每日额度|
|R18PunishTime|300|触发R18的禁用时间(秒)|
|APIBaseUrl|http://127.0.0.1:7860/|API基础网址，需要末尾的/|
|Steps|30|绘画采样步数|
|Height|896|生成图片的高度|
|Width|896|生成图片的宽度|
|Timeout|300|调用最大超时时长(秒)|
|RestoreFaces|true|RestoreFaces|
|SamplingMethod|Euler a|采样算法|
|NegativePrompt|Lowres, bad anatomy, bad hands, text, error, missing, fingers, extra digit, fewer digits, cropped, worst, quality, Low quality, normal quality, jpeg , artifacts, signature, watermark, username, blurry, bad feet||
|BasePrompt|{{masterpiece}}, {{best quality}}, extremely detailed CG unity 8k wallpaper|调用的prompt将在这个后面补充|
|CallResponse|今日可用次数: %count%\\n开始作画，大概需要1分钟...|触发调用时的回复|
|BusyResponse|当前有任务正在进行，请等待前一任务完成...|有任务正在进行时的回复|
|NoQuotaResponse|调用额度达到上限|个人额度上限的回复|
|R18PunishResponse|触发R18审计配置，禁用%time%秒|R18惩罚触发时的回复|
|Txt2Img|#作画|txt2img的指令|
|Img2Img|#转图|img2img的指令|
|UseTranslate|true|是否对prompt进行翻译|
|Baidu_AppId||百度翻译的APPID|
|Baidu_Key||百度翻译的APPKey|
|TranslateType|Baidu|使用的翻译API(目前只有百度翻译)|


## 待完成的功能
1. [ ] img2img
2. [ ] 通过指令设置/查看配置
3. [ ] 检测WebUI是否在运行
4. [ ] 可通过指令关闭/启动/重启WebUI
5. [ ] 群额度
6. [ ] 黑白群名单
7. [ ] NSFW过滤
8. [ ] tag查询
9. [ ] 禁用时段
10. [ ] 使用样式快速生成

## 已知的bug
