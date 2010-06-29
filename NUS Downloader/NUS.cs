using System;
using System.Collections.Generic;
using System.Text;

namespace NUS_Downloader
{
    class NUS
    {
        // NUS Urls
        const string NUS_Wii = "http://nus.cdn.shop.wii.com/ccs/download/";
        const string NUS_DSi = "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/";

        // NUS Files
        const string Ticket = "cetk";
        const string TMD = "tmd";

        // Report Status back in EventHandler
        public delegate void StatusChangedEventHandler(string status);
        //public event StatusChangedEventHandler StatusChanged;
    }
}
