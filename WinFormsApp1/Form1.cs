using Microsoft.VisualBasic.FileIO;
using System.Collections;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Text;
using static WinFormsApp1.CSVFile;
using static WinFormsApp1.INIFile;

namespace WinFormsApp1
{
    /// <summary>
    /// INI File class handle
    /// </summary>
    public class INIFile
    {
        public const string STR_DEFINE_KEY_VALUE_INVALID_EMPTY = "";
        public const string STR_DEFINE_KEY_VALUE_INVALID_NONSET = "-";

        public string path { get; set; }
        public ArrayList listSections = new ArrayList(); 
        public ArrayList listSubSections = new ArrayList();

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames")]
        private static extern int GetSectionNamesList(byte[] lpszReturnBuffer, int nSize, string lpFileName);
        public INIFile(string INIPath)
        {
            path = INIPath;
        }
        public INIFile()
        {}

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, STR_DEFINE_KEY_VALUE_INVALID_EMPTY, temp, 255, this.path);
            return temp.ToString();
        }


        public void GetSectionNames()
        {
            //Get The Sections List Method

            this.listSections.Clear();
            byte[] buff = new byte[1024];
            GetSectionNamesList(buff, buff.Length, this.path);
            String s = Encoding.Default.GetString(buff);
            String[] names = s.Split('\0');
            foreach (String name in names)
            {
                if (name != String.Empty)
                {
                    this.listSections.Add(name);
                }
            }

            // filter sub section by name
            this.listSubSections.Clear();
            foreach (string sec in this.listSections)
            {
                int count = CountOccurences(sec, "_");
                if (count > 3)
                {
                    this.listSubSections.Add(sec);
                }
            }
        }
        public int CountOccurences(string source, string substring)
        {
            int count = 0, n = 0;

            if (substring != "")
            {
                while ((n = source.IndexOf(substring, n, StringComparison.InvariantCulture)) != -1)
                {
                    n += substring.Length;
                    ++count;
                }
            }
            return count;
        }
    }

    /// <summary>
    /// CSV file class handle
    /// </summary>
    public class CSVFile
    {
        public const string STR_DEFINE_SECTION_HEADER_NAME = "section";

        public const string STR_DEFINE_KEY_POSX = "POSX";
        public const string STR_DEFINE_KEY_POSY = "POSY";
        public const string STR_DEFINE_KEY_HEIGHT = "HEIGHT";
        public const string STR_DEFINE_KEY_WIDTH = "WIDTH";
        public const string STR_DEFINE_KEY_FONT_HIGHT = "FONT_HIGHT";


        public string path { get; set; }
        public int indexSection { get; set; }
        public List<string> listHeader = new List<string>();
        public List<SectionUpdateInfomation> allSectionCsvData = new List<SectionUpdateInfomation>();


        public class SectionUpdateInfomation
        {
            public List<string> csvRowData = new List<string>();
            public List<string> sectionLists = new List<string>();
            public SectionUpdateInfomation()
            { }
        }
        public CSVFile()
        { }
        public void ParseCSV()
        {
            using (TextFieldParser textFieldParser = new TextFieldParser(path))
            {
                bool b1stline = true;
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");
                while (!textFieldParser.EndOfData)
                {
                    string[] rows = textFieldParser.ReadFields();
                    if (b1stline)
                    {
                        // parse header
                        b1stline = false;
                        listHeader = rows.ToList();
                        //get index of section
                        for (int i = 0; i < listHeader.Count; i++)
                        {
                            if (listHeader[i] == STR_DEFINE_SECTION_HEADER_NAME)
                            {
                                this.indexSection = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // parse data
                        SectionUpdateInfomation rowData = new SectionUpdateInfomation();
                        rowData.csvRowData = rows.ToList();
                        allSectionCsvData.Add(rowData);
                    }

                }
            }
        }

        public void SetSectionUpdateList(ref ArrayList alSubSections)
        {
            string strSubSectionPrefix = "";
            string strCurSectionName = "";
            foreach (var currentCSVSection in allSectionCsvData)
            {
                // Add current section
                strCurSectionName = currentCSVSection.csvRowData[indexSection];
                currentCSVSection.sectionLists.Add(strCurSectionName);

                // Add sub section by checking string prefix "_"
                strSubSectionPrefix = strCurSectionName + "_";
                foreach (string strSubSectionName in alSubSections.ToArray())
                {
                    // remove invalid sub section name
                    if (string.IsNullOrEmpty(strSubSectionName))
                    {
                        alSubSections.Remove(strSubSectionName);
                        continue;
                    }

                    // check related to add sub section
                    if (strSubSectionName.Contains(strSubSectionPrefix) ) 
                    {
                        currentCSVSection.sectionLists.Add(strSubSectionName);
                        // remove sub section after adding
                        // current loop in a copy array, therefore the base array can delete items (Performance issue)
                        alSubSections.Remove(strSubSectionName);
                    }
                }
            }
        }

        }


        public partial class Form1 : Form
    {
        /// ///////////////////
        /// GLOBAL DATA
        //////////////////////


        //////////////////////
        //////////////////////

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                this.textBox1.Text = files[0];
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            INIFile objIni = new INIFile();
            CSVFile objCsv = new CSVFile();

            // Get ini infor
            objIni.path = this.textBox1.Text;
            this.textBox2.Text = objIni.path;
            objIni.IniWriteValue("objects", "OBJ_009_VALUE", "•s•s•s");
            objIni.GetSectionNames();

            // get csv infor
            objCsv.path = this.textBox2.Text;
            objCsv.ParseCSV();
            objCsv.SetSectionUpdateList(ref objIni.listSubSections);

            // update ini by csv information
            UpdateIniData(ref objIni, ref objCsv);

            Console.WriteLine("");
        }

        //////////
        //////////
        private void UpdateIniData(ref INIFile refIni,ref CSVFile refCsv)
        {
            foreach (SectionUpdateInfomation objSectionData in refCsv.allSectionCsvData)
            {
                foreach (string sSectionName in objSectionData.sectionLists)
                {
                    // loop set data from position after row section to the end
                    for (int iHeader = refCsv.indexSection + 1; iHeader < refCsv.listHeader.Count; iHeader++)
                    {
                        string sKeyname = refCsv.listHeader[iHeader];
                        string sNewValue = objSectionData.csvRowData[iHeader];

                        // Key available for sure, no need to check.
                        if ( (sKeyname == STR_DEFINE_KEY_POSX)  ||
                             (sKeyname == STR_DEFINE_KEY_POSY)  ||
                             (sKeyname == STR_DEFINE_KEY_WIDTH) ||
                             (sKeyname == STR_DEFINE_KEY_WIDTH) )
                        {
                            refIni.IniWriteValue(sSectionName, sKeyname, sNewValue);
                        }
                        else 
                        {
                            // Key may be not available, Check valid before set
                            // Check available key before set
                            string sCurValue = STR_DEFINE_KEY_VALUE_INVALID_EMPTY;
                            sCurValue = refIni.IniReadValue(sSectionName, sKeyname);
                            if (sCurValue == STR_DEFINE_KEY_VALUE_INVALID_EMPTY)
                            {
                                // invalid key;
                                continue;
                            }
                            // Check valid new value
                            if ( (sNewValue == STR_DEFINE_KEY_VALUE_INVALID_EMPTY) ||
                                 (sNewValue == STR_DEFINE_KEY_VALUE_INVALID_NONSET) )
                            {
                                // invalid new value;
                                continue;
                            }


                            // Set new value to ini
                            refIni.IniWriteValue(sSectionName, sKeyname, sNewValue);
                        }

                    }
                }
            }

        }
    }
}
