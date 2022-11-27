﻿using System.Net.Sockets;

namespace GOI
{
    public class THUONG
    {
        public THUONG(string kind, string? content)
        {
            this.kind = kind;
            this.content = content;
        }
        public string kind { get; set; }
        public string? content { get; set; }
    }

    public class LOGIN
    {
        public LOGIN(string? username, string? pass)
        {
            this.username = username;
            this.pass = pass;
        }
        public string? username { get; set; }
        public string? pass { get; set; }
    }

    public class TINNHAN
    {
        public TINNHAN(string? usernameSender, string? usernameReceiver, string? content)
        {
            this.usernameSender = usernameSender;
            this.usernameReceiver = usernameReceiver;
            this.content = content;
        }
        public string? usernameSender { get; set; }
        public string? usernameReceiver { get; set; }
        public string? content { get; set; }
    }

    public class GETMES
    {
        public GETMES(string? sender, string? receiver)
        {
            this.sender = sender;
            this.receiver = receiver;
        }

        public string? sender { get; set; }
        public string? receiver { get; set; }
    }

    public class GUIHINH
    {
        public GUIHINH(string? usernameSender, string? usernameReceiver, byte[]? manghinh, string tenhinh)
        {
            this.usernameSender = usernameSender;
            this.usernameReceiver = usernameReceiver;
            this.manghinh = manghinh;
            this.tenhinh = tenhinh;
        }
        public string? usernameSender { get; set; }
        public string? usernameReceiver { get; set; }
        public byte[]? manghinh { get; set; }

        public string? tenhinh { get; set; }
    }

    public class LAYHINH
    {
        public LAYHINH(string? useryeucau, string? path, string type)
        {
            this.path = path;
            this.useryeucau = useryeucau;
            this.type = type;
        }
        public string? path { get; set; }

        public string? useryeucau { get; set; }

        public string? type { get; set; }
    }

    public class TRAHINH
    {
        public TRAHINH(byte[]? manghinh, string? type)
        {
            this.manghinh = manghinh;
            this.type = type;
        }

        public byte[]? manghinh { get; set; }

        public string? type { get; set; }
    }
}