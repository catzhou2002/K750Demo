# K750Demo

[K750](http://www.szttce.com/product_show.asp?p=288)是深圳天腾实业有限公司生产的一款能循环收发各类IC卡的机器，天腾提供了DLL供调用，有些问题：
1. 只能32位调用
2. 不支持多线程
3. 当然还必须带着他提供的DLL

不爽，直接调用Usb Hid读写，完成了封装，变成了纯洁的C#，优点：
1. 32位、64位随便
2. 多线程，特别是开个线程判断机器的状态，特爽
3. 没有多余的Dll
当然还有一种掌控的感觉。

封装的过程，遇到一些坑，吐槽一下天腾：
1. 地址编码：16个地址，用了两个字节，用字符‘0’‘0’来代表0号机，‘1’‘5’来代表15号机，我也是醉了
2. 机器状态编码：16中状态，用了四个字节，套路同上，感觉很受伤

