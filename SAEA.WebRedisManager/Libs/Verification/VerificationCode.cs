/****************************************************************************
*项目名称：SAEA.WebRedisManager.Libs.Verification
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Libs.Verification
*类 名 称：VerificationCode
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/5/9 11:33:40
*描述：
*=====================================================================
*修改时间：2020/5/9 11:33:40
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.MVC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace SAEA.WebRedisManager.Libs.Verification
{
    public class VerificationCode
    {
        private AnimatedGifEncoder coder = new AnimatedGifEncoder();

        private char[] _identifyingCode;

        private int _defaultIdentifyingCodeLen = 4;

        const string _availableLetters1 = @"的一是在了不和有大这主中人上为们地个用工时要动国产以我到他会作来分生对于学下级就年阶义发成部民可出能方进同行面说种过命度革而多子后自社加小机也经力线本电高量长党得实家定深法表着水理化争现所二起政三好十战无农使性前等反体合斗路图把结第里正新开论之物从当两些还天资事队批如应形想制心样干都向变关点育重其思与间内去因件日利相由压员气业代全组数果期导平各基或月毛然问比展那它最及外没看治提五解系林者米群头意只明四道马认次文通但条较克又公孔领军流入接席位情运器并飞原油放立题质指建区验活众很教决特此常石强极土少已根共直团统式转别造切九你取西持总料连任志观调七么山程百报更见必真保热委手改管处己将修支识病象几先老光专什六型具示复安带每东增则完风回南广劳轮科北打积车计给节做务被整联步类集号列温装即毫知轴研单色坚据速防史拉世设达尔场织历花受求传口断况采精金界品判参层止边清至万确究书术状厂须离再目海交权且儿青才证低越际八试规斯近注办布门铁需走议县兵固除般引齿千胜细影济白格效置推空配刀叶率述今选养德话查差半敌始片施响收华觉备名红续均药标记难存测士身紧液派准斤角降维板许破述技消底床田势端感往神便贺村构照容非搞亚磨族火段算适讲按值美态黄易彪服早班麦削信排台声该击素张密害侯草何树肥继右属市严径螺检左页抗苏显苦英快称坏移约巴材省黑武培著河帝仅针怎植京助升王眼她抓含苗副杂普谈围食射源例致酸旧却充足短划剂宣环落首尺波承粉践府鱼随考刻靠够满夫失包住促枝局菌杆周护岩师举曲春元超负砂封换太模贫减阳扬江析亩木言球朝医校古呢稻宋听唯输滑站另卫字鼓刚写刘微略范供阿块某功套友限项余倒卷创律雨让骨远帮初皮播优占死毒圈伟季训控激找叫云互跟裂粮粒母练塞钢顶策双留误础吸阻故寸盾晚丝女散焊功株亲院冷彻弹错散商视艺灭版烈零室轻血倍缺厘泵察绝富城冲喷壤简否柱李望盘磁雄似困巩益洲脱投送奴侧润盖挥距触星松送获兴独官混纪依未突架宽冬章湿偏纹吃执阀矿寨责熟稳夺硬价努翻奇甲预职评读背协损棉侵灰虽矛厚罗泥辟告卵箱掌氧恩爱停曾溶营终纲孟钱待尽俄缩沙退陈讨奋械载胞幼哪剥迫旋征槽倒握担仍呀鲜吧卡粗介钻逐弱脚怕盐末阴丰编印蜂急拿扩伤飞露核缘游振操央伍域甚迅辉异序免纸夜乡久隶缸夹念兰映沟乙吗儒杀汽磷艰晶插埃燃欢铁补咱芽永瓦倾阵碳演威附牙芽永瓦斜灌欧献顺猪洋腐请透司危括脉宜笑若尾束壮暴企菜穗楚汉愈绿拖牛份染既秋遍锻玉夏疗尖殖井费州访吹荣铜沿替滚客召旱悟刺脑措贯藏敢令隙炉壳硫煤迎铸粘探临薄旬善福纵择礼愿伏残雷延烟句纯渐耕跑泽慢栽鲁赤繁境潮横掉锥希池败船假亮谓托伙哲怀割摆贡呈劲财仪沉炼麻罪祖息车穿货销齐鼠抽画饲龙库守筑房歌寒喜哥洗蚀废纳腹乎录镜妇恶脂庄擦险赞钟摇典柄辩竹谷卖乱虚桥奥伯赶垂途额壁网截野遗静谋弄挂课镇妄盛耐援扎虑键归符庆聚绕摩忙舞遇索顾胶羊湖钉仁音迹碎伸灯避泛亡答勇频皇柳哈揭甘诺概宪浓岛袭谁洪谢炮浇斑讯懂灵蛋闭孩释乳巨徒私银伊景坦累匀霉杜乐勒隔弯绩招绍胡呼痛峰零柴簧午跳居尚丁秦稍追梁折耗碱殊岗挖氏刃剧堆赫荷胸衡勤膜篇登驻案刊秧缓凸役剪川雪链渔啦脸户洛孢勃盟买杨宗焦赛旗滤硅炭股坐蒸凝竟陷枪黎救冒暗洞犯筒您宋弧爆谬涂味津臂障褐陆啊健尊豆拔莫抵桑坡缝警挑污冰柬嘴啥饭塑寄赵喊垫康遵牧遭幅园腔订香肉弟屋敏恢忘衣孙龄岭骗休借丹渡耳刨虎笔稀昆浪萨茶滴浅拥穴覆伦娘吨浸袖珠雌妈紫戏塔锤震岁貌洁剖牢锋疑霸闪埔猛诉刷狠忽灾闹乔唐漏闻沈熔氯荒茎男凡抢像浆旁玻亦忠唱蒙予纷捕锁尤乘乌智淡允叛畜俘摸锈扫毕璃宝芯爷鉴秘净蒋钙肩腾枯抛轨堂拌爸循诱祝励肯酒绳穷塘燥泡袋朗喂铝软渠颗惯贸粪综墙趋彼届墨碍启逆卸航雾冠丙街莱贝辐肠付吉渗瑞惊顿挤秒悬姆烂森糖圣凹陶词迟蚕亿矩";

        const string _availableLetters2 = "1234567890";

        const string _availableLetters3 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        int _charType = 1;

        private Random _random = new Random();

        private Color _color;


        private int _frameCount = 6;
        private int _delay = 500;
        private int _noiseCount = 15;

        private int _width = 150, _height = 60;
        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public string IdentifyingCode
        {
            get { return new string(_identifyingCode); }
        }

        public VerificationCode(int width, int height, int len = 4, int charType = 0)
        {
            _width = width < 1 ? 1 : width;
            _height = height < 1 ? 1 : height;
            _defaultIdentifyingCodeLen = len;
            _charType = charType;
            coder.SetSize(Width, Height);
            coder.SetRepeat(0);
        }

        private void GenerateIdentifyingCode(int codeLength)
        {
            if (codeLength < 1)
                codeLength = 4;

            List<char> codes = new List<char>();

            var l1 = _availableLetters1;
            var l2 = _availableLetters2;
            var l3 = _availableLetters3;
            var l4 = l1 + l2;
            var l5 = l1 + l3;
            var l6 = l2 + l3;
            var l7 = l1 + l2 + l3;


            for (int i = 0; i < codeLength; i++)
            {
                switch (_charType)
                {
                    case 1:
                        codes.Add(l1[_random.Next(0, l1.Length)]);
                        break;
                    case 2:
                        codes.Add(l2[_random.Next(0, l2.Length)]);
                        break;
                    case 3:
                        codes.Add(l4[_random.Next(0, l4.Length)]);
                        break;
                    case 4:
                        codes.Add(l3[_random.Next(0, l3.Length)]);
                        break;
                    case 5:
                        codes.Add(l5[_random.Next(0, l5.Length)]);
                        break;
                    case 6:
                        codes.Add(l6[_random.Next(0, l6.Length)]);
                        break;
                    default:
                        codes.Add(l7[_random.Next(0, l7.Length)]);
                        break;
                }

            }

            _identifyingCode = new char[codes.Count];

            codes.CopyTo(_identifyingCode);
        }

        public Stream Create(Stream stream)
        {
            GenerateIdentifyingCode(_defaultIdentifyingCodeLen);

            coder.Start(stream);
            Process(IdentifyingCode);
            return stream;
        }

        public MemoryStream Create()
        {
            GenerateIdentifyingCode(_defaultIdentifyingCodeLen);

            MemoryStream stream = new MemoryStream();
            coder.Start(stream);
            Process(IdentifyingCode);
            return stream;
        }

        private void Process(string str)
        {
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            Font f = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            for (int i = 0; i < _frameCount; i++)
            {
                Image im = new Bitmap(Width, Height);

                _color = Color.FromArgb(_random.Next(128, 256), _random.Next(128, 256), _random.Next(128, 256));

                var bb = new SolidBrush(_color);

                Graphics ga = Graphics.FromImage(im);

                ga.FillRectangle(bb, rect);

                int fH = (int)f.GetHeight();

                int fW = (int)ga.MeasureString(str, f).Width;

                _color = Color.FromArgb(_random.Next(0, 128), _random.Next(0, 128), _random.Next(0, 128));

                var fb = new SolidBrush(_color);

                AddNoise(ga, _color);

                ga.DrawString(str, f, fb, new PointF(_random.Next(1, Width - 1 - fW), _random.Next(1, Height - 1 - fH)));

                ga.Flush();

                coder.SetDelay(_delay);

                coder.AddFrame(im);

                im.Dispose();
            }
            coder.Finish();
        }

        private void AddNoise(Graphics ga, Color color)
        {

            Pen pen = new Pen(color);

            Point[] ps = new Point[_noiseCount];

            for (int i = 0; i < _noiseCount; i++)
            {
                ps[i] = new Point(_random.Next(1, Width - 1), _random.Next(1, Height - 1));
            }

            ga.DrawLines(pen, ps);
        }

        public void Create(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            coder.Start(fs);
            Process(IdentifyingCode);
            fs.Close();
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Body = new byte[0];
            context.Response.ContentType = "image/Gif";
            var stream = Create();
            context.Response.BinaryWrite(stream.ToArray());
            stream.Dispose();
        }
    }
}
