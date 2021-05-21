using Farm.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Farm.Views
{
    [QueryProperty(nameof(Key), nameof(Key))]
    [QueryProperty(nameof(DisInfo), nameof(DisInfo))]
    public partial class FarmDetailPage : ContentPage
    {

        public string DisInfo
        {
            set
            {
                DisclosureInfomation info = JsonConvert.DeserializeObject<DisclosureInfomation>(value);
                공시번호.Text   = info.공시번호;
                상표명.Text     = info.상표명;
                자재구분.Text   = info.자재구분;
                등재일자.Text   = info.등재일자;
                공시기간.Text   = info.공시기간;
                제조업체.Text   = info.제조업체명;
                가격.Text       = info.가격;
                대표자명.Text   = info.대표자명;
                사업장주소.Text = info.사업장주소;
                관리기관명.Text = info.관리기관명;
            }
        }
        public string Key 
        {
            set
            {
                double defaultHeight = 30;
                Double.TryParse(value, out defaultHeight);
                //Header.HeightRequest = defaultHeight;
            }
        }
        async void BackButtonPressed(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
        public FarmDetailPage()
        {
            InitializeComponent();

            //disinfo = App.DisInfo[0];

            //int rowIndex = 0;
            //foreach (var prop in disinfo.GetType().GetProperties())
            //{
            //    Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(disinfo, null));
            //    if(prop.Name == "ROW_NUM" || prop.Name =="공시ID" || prop.Name.Contains("공시만료일") || prop.Name.Contains("공시시작") || prop.Name.Contains("공시종료"))
            //    {
            //        continue;
            //    }

            //    Label name = new Label
            //    {
            //        Text = prop.Name,
            //        TextColor = Color.Black
            //    };
            //    name.SetValue(Grid.RowProperty, rowIndex);
            //    ContentsGrid.Children.Add(name);

            //    object objValue = prop.GetValue(disinfo, null);
            //    if (objValue != null)
            //    {
            //        Label value = new Label
            //        {
            //            Text = objValue.ToString(),
            //            TextColor = Color.Black
            //        };
            //        value.SetValue(Grid.RowProperty, rowIndex);
            //        value.SetValue(Grid.ColumnProperty, 1);
            //        ContentsGrid.Children.Add(value);
            //    }
            //    rowIndex++;
            //}
        }
    }
}