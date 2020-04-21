using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot;
using System.IO;
using System.Windows.Automation;
using System.Net;
using System.Net.Http;

namespace ChatBot_v._4_Telegram_Sql
{
    class Msg
    {
        public long id;
        public string name;
        public string text;

        public override string ToString()
        {
            return $"{name}: {text}";
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TelegramBotClient bot;
        WebProxy proxy;
        HttpClientHandler httpClientHandler;
        HttpClient hc;

        public MainWindow()
        {
            InitializeComponent();

            proxy = new WebProxy(Address: new Uri("http://163.172.189.32:8811"));
            httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy
            };
            hc = new HttpClient(handler: httpClientHandler, disposeHandler: true);

            bot = new TelegramBotClient(File.ReadAllText("token.txt"));
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Msg currentMsg = new Msg()
            {
                id = e.Message.Chat.Id,
                name = e.Message.Chat.FirstName,
                text = e.Message.Text
            };
            DataBaseClass.Add(currentMsg);

            this.Dispatcher.Invoke(()=> listMsg.Items.Add(currentMsg));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            long id = ((Msg)listMsg.SelectedItem).id;

            bot.SendTextMessageAsync(id, tbSendMessage.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataBaseClass.Free();
        }
    }
}
