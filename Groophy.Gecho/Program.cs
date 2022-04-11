using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Groophy.Gecho
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "/h" || args[0] == "/?" || args[0] == "-h" || args[0] == "--help" || args[0] == "help")
                {
                    h();
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int i = 0; i < args.Length; i++) sb.Append(args[i] + " ");
                    sb.Remove(sb.Length - 1, 1);
                    string[] lns = sb.ToString().Split(new[] { "<nl>" }, StringSplitOptions.None);
                    for (int i = 0;i < lns.Length;i++) Coologs.Print(lns[i]);
                    Console.ResetColor();
                }
            }
            else
            {
                h();
            }
        }

        static void h()
        {
            Console.WriteLine(@"
Gecho helps you write colorful texts in one line with low ms guarantee.

Syntax: 
	call gecho"+" \"<COLORS> TEXT\""+@"

	call gecho "+"\"This is a <r>Red<gn>Green<gray>Grey<Bk>Black<B>Blue\""+@"

Note:
	Because of cmd's rules, you must enclose the string in double quotes for the command to work properly.

Where:
        <Black> = Black         <Gray> = Gray(g)
        <Blue> = Blue(b)        <DarkBlue> = Dark Blue(db)
        <Green> = Green(gn)     <DarkCyan>  = Dark Cyan(dc)
        <Cyan> = Cyan(c)        <DarkGray> = Dark Gray(dgy)
        <Red> = Red(r)          <DarkGreen> = Dark Green(dgn)
        <White> = White(w)      <DarkMagenta> = Dark Magenta(dm)
        <Yellow> = Yellow(y)    <DarkRed> = Dark Red(dr)
        <Magenta> = Magenta(m)  <DarkYellow> = Dark Yellow(dy)
        </> = Reset Color       <nl> = NewLine without reset color

AUTHOR:
	Gecho has been written by Groophy Lifefor and helped by MBausson.
	Discord: Groophy#1966
");
            Console.ReadKey();
        }
    }

    public static class Coologs
    {
        /// <summary>
        /// Prints a text and applies foreground changes with &lt;colors&gt; tags in it.
        /// </summary>
        /// <param name="text_code">Text desired with print styling code</param>
        /// <param name="endwith">Character at the end of the print -- default is a line break</param>
        /// <example>Coologs.Print("&lt;red&gt;Red text<blue> Blue text</blue> Normal text");</example>
        /// <remarks>You can escape the character '&lt;' with '&lt;' (Example: '&lt;&lt;test&gt;'</remarks>
        public static void Print(string text_code, string endwith = "\n")
        {
            bool parsing_tag = false;
            string tag = "";

            for (int i = 0; i < text_code.Length; i++)
            {
                if (parsing_tag)
                {

                    if (text_code[i] == '>')
                    {
                        parsing_tag = false;
                        tag = tag.ToLower();
                        tag = tag.Substring(1);

                        //  Check for escape character '<'
                        if (tag[0] == '<')
                        {
                            Console.Write(tag + '>');   //  We rebuild inital escaped tag because we previously remove the last '>'
                            tag = "";
                            continue;
                        }

                        //  A </> closure tag resets color
                        if (tag[0] == '/')
                        {
                            Console.ResetColor();
                        }
                        //  Sets desired color
                        else
                        {
                            //  If the color code is incorrect, we don't raise any error -- just let it go
                            int color = getColor(tag);

                            if (color != -1)
                            {
                                Console.ForegroundColor = (ConsoleColor)color;
                            }
                        }

                        tag = "";
                        continue;
                    }

                    tag += text_code[i];
                    continue;
                }

                if (text_code[i] == '<')
                {
                    parsing_tag = true;
                    tag += "<";
                    continue;
                }

                Console.Write(text_code[i].ToString());
            }

            Console.Write(endwith);
        }

        /// <summary>
        /// Returns color's int code based on its string representation
        /// </summary>
        /// <param name="code">Color's string code</param>
        /// <remarks>Returns -1 if the input doesn't match any color.</remarks>
        private static int getColor(string code)
        {
            if (code == "b") code = "blue";
            else if (code == "c") code = "cyan";
            else if (code == "db") code = "darkblue";
            else if (code == "dc") code = "darkcyan";
            else if (code == "dgn") code = "darkgreen";
            else if (code == "dgy") code = "darkgray";
            else if (code == "dm") code = "darkmagenta";
            else if (code == "dr") code = "darkred";
            else if (code == "dy") code = "darkyellow";
            else if (code == "gy") code = "gray";
            else if (code == "gn") code = "green";
            else if (code == "m") code = "m";
            else if (code == "r") code = "red";
            else if (code == "w") code = "white";
            else if (code == "y") code = "yellow";

            foreach (int i in Enum.GetValues(typeof(ConsoleColor)))
            {
                if (((ConsoleColor)i).ToString().ToLower() == code)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
