using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Encoder = System.Drawing.Imaging.Encoder;

namespace TAPSONIC_TOP
{
    class Program
    {
        static int mt = 4;// 스킬 영역 크기 배율
        static int ch1 = 15; // 스킬 표시 세로 범위
        static int ch2 = 5;
        static int ch = ch1 + ch2;//캐릭별 세로 크기
        static int tw = 80;// 텍스트 영역 가로 크기
        static int sw = 180 * mt;// 스킬 영역 가로 크기

        Program()
        {            
        }

        enum Att
        {
            dancer,
            session,
            vocal
        }

        // 캐릭별 off on 시간 (초)
        struct Chra
        {
            public string name;
            public Att att;
            public int off;
            public int on;

            public Chra(string name, Att att, int off, int on)
            {
                this.name = name;
                this.att = att;
                this.off = off*mt;
                this.on = on*mt;                
            }

            public override string ToString() => $"({name}, {off}, {on})";
        }


        static void Main(string[] args)
        {


            // 출력파일명 설정
            StringBuilder sb = new StringBuilder();
            sb.Append(@"TAPSONIC_TOP_");
            sb.Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
            sb.Append(@".png");
            Console.WriteLine(sb.ToString());

            // 색상 설정
            Color con= Color.FromArgb(255, 255, 0);     
            Color coff= Color.FromArgb(255, 255, 0);
            Color cdancer = Color.FromArgb(255, 0, 0);
            Color csession = Color.FromArgb(0, 255, 0);
            Color cvocal = Color.FromArgb(0, 0, 255);

            // 캐릭 목록
            List<Chra> cd = new List<Chra>();
            cd.Add(new Chra("엘리10", Att.vocal, 18, 14));
            cd.Add(new Chra("제시10", Att.vocal, 12, 13));
            cd.Add(new Chra("엘클리어10", Att.vocal, 18, 10));
            cd.Add(new Chra("아리아10", Att.vocal, 16, 10));
            cd.Add(new Chra("니콜10", Att.vocal, 15, 13));
            cd.Add(new Chra("걸윙10", Att.vocal, 14, 10));
            cd.Add(new Chra("셜리10", Att.dancer, 17, 10));
            cd.Add(new Chra("라파엘10", Att.dancer, 16, 18));
            cd.Add(new Chra("크라켄10", Att.dancer, 16, 10));
            cd.Add(new Chra("호련10", Att.dancer, 15, 15));
            cd.Add(new Chra("벨10", Att.dancer, 21, 10));
            cd.Add(new Chra("루시퍼10", Att.session, 15, 14));
            cd.Add(new Chra("트래셔10", Att.session, 19, 15));
            cd.Add(new Chra("재규어10", Att.session, 15, 11));
            cd.Add(new Chra("업튼10", Att.session, 18, 14));
            cd.Add(new Chra("틴체르니10", Att.session, 17, 10));
            cd.Add(new Chra("볼프강10", Att.session, 10, 9));

            // 1. Create a bitmap
            using (Bitmap bitmap = new Bitmap(tw+sw, ch * cd.Count, PixelFormat.Format32bppArgb))
            {
                var graphics = Graphics.FromImage(bitmap);
                var fontb = new Font("맑은 고딕", 16, FontStyle.Bold, GraphicsUnit.Pixel);
                var fontf = new Font("맑은 고딕", 16, FontStyle.Regular, GraphicsUnit.Pixel);
                int cnt=0;
                Brush b= Brushes.Black;

                // 기본 영역
                graphics.FillRectangle(Brushes.Black,
                    0,
                    0,
                    tw + sw , ch * cd.Count); //line

                // 세로 경계선
                graphics.FillRectangle(Brushes.White,
                    tw - ch2,
                    0,
                    ch2, ch * cd.Count); //line

                // 캐릭마다 색 설정
                cd.ForEach(delegate (Chra c) {
                    switch (c.att)
                    {
                        case Att.dancer:
                            b = Brushes.Red;
                            break;
                        case Att.session:
                            b = Brushes.Green;
                            break;
                        case Att.vocal:
                            b = Brushes.Blue;
                            break;
                        default:
                            b = Brushes.Black;
                            break;
                    }

                    // 그리기 반복
                    for (int i = tw; i < sw+tw; i+=c.off+c.on)
                    {                    
                        //graphics.FillRectangle(Brushes.Black,
                        //    i,
                        //    20*cnt,
                        //    c.off, ch1); //off
                        graphics.FillRectangle(Brushes.Yellow,
                            i + c.off,
                            20 * cnt,
                            c.on, ch1); //on
                    }

                    // 속성라인 그리기
                    graphics.FillRectangle(b,
                        tw,
                        20 * cnt + ch1,
                        sw, ch2); //line

                    // 10초단위 그리기 반복
                    for (int i = tw + 9 * mt; i < sw + tw; i += 10 * mt)
                    {
                        graphics.FillRectangle(Brushes.White,
                            i,
                            20 * cnt + ch1,
                            mt, ch2); //on
                    }
                    // 60초단위 그리기 반복
                    for (int i = tw + 59 * mt; i < sw + tw; i += 60 * mt)
                    {
                        graphics.FillRectangle(Brushes.Black,
                            i,
                            20 * cnt + ch1,
                            mt, ch2); //on
                    }

                    // 이름
                    //graphics.DrawString(c.name, fontb, Brushes.Black, 0, 20 * cnt);
                    graphics.DrawString(c.name, fontf, Brushes.White, 0, 20 * cnt);
                    cnt++;
                });

                bitmap.Save(sb.ToString(), ImageFormat.Png);
            }



            //Console.ReadLine();
        }
    }
}
