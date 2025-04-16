using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConnectionAPI
{
    [Serializable]
    public abstract class ServerCommand
    {
        public string Header { get; set; }

        protected ServerCommand(string header)
        {
            Header = header;
        }
    }

    [Serializable]
    public class GetBooksCommand : ServerCommand
    {
        public static string StaticHeader = "GetBooks";

        public GetBooksCommand() : base(StaticHeader) { }
    }


    [Serializable]
    public class SubscribeToNewsletterUpdatesCommand : ServerCommand
    {
        public static string StaticHeader = "SubscribeToNewsletterUpdates";

        public bool Subscribed { get; set; }

        public SubscribeToNewsletterUpdatesCommand() : base(StaticHeader) { }
    }

    [Serializable]
    public class SellBookCommand : ServerCommand
    {
        public static string StaticHeader = "SellBook";

        public int BookID { get; set; }
        public string Username { get; set; } = "";

        public SellBookCommand() : base(StaticHeader) { }
        public SellBookCommand(int id, string username) : base(StaticHeader)
        {
            BookID = id;
            Username = username;
        }
    }

    [Serializable]
    public class GetUserCommand : ServerCommand
    {
        public static string StaticHeader = "GetUser";

        public string Username { get; set; }

        public GetUserCommand(string username) : base(StaticHeader)
        {
            Username = username;
        }
    }
    [Serializable]
    public struct BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; }
        public int Genre { get; set; }

        public BookDTO(int Id, string title, string author, decimal Price, bool Recommended, int Genre)
        {
            this.Id = Id;
            this.Title = title;
            this.Author = author;
            this.Price = Price;
            this.Recommended = Recommended;
            this.Genre = Genre;
        }
    }

    [Serializable]
    public struct UserDTO
    {
        public string Username { get; set; }
        public decimal Balance { get; set; }
        public BookDTO[] Books { get; set; }

        public UserDTO(string username, decimal balance, BookDTO[] booksId)
        {
            this.Username = username;
            this.Balance = balance;
            this.Books = booksId;
        }
    }

    [Serializable]
    public abstract class ServerResponse
    {
        public string Header { get; private set; }

        protected ServerResponse(string header)
        {
            Header = header;
        }
    }
    [Serializable]
    public class AllBooksUpdateResponse : ServerResponse
    {
        public static readonly string StaticHeader = "AllBooksUpdate";

        public BookDTO[]? Books { get; set; }

        public AllBooksUpdateResponse() : base(StaticHeader) { }
    }

    [Serializable]
    public class BookChangedResponse : ServerResponse
    {
        public static readonly string StaticHeader = "BookChanged";

        public BookDTO Book { get; set; }
        public int ChangeType { get; set; }

        public BookChangedResponse() : base(StaticHeader) { }
        public BookChangedResponse(int change) : base(StaticHeader) { ChangeType = change; }
    }


    [Serializable]
    public class TransactionResultResponse : ServerResponse
    {
        public static readonly string StaticHeader = "TransactactionResult";

        public int BookID { get; set; }
        public string Username { get; set; }
        public int ResultCode { get; set; }
        // 0 - success 1 - not enought money 2 - book not found 3 - user not found 4 - unknown error

        public TransactionResultResponse(int bookId, string username, int resultCode) : base(StaticHeader)
        {
            BookID = bookId;
            Username = username;
            ResultCode = resultCode;
        }
    }

    [Serializable]
    public class UserChangedResponse : ServerResponse
    {
        public static readonly string StaticHeader = "UserChanged";

        public UserDTO User { get; set; }

        public UserChangedResponse() : base(StaticHeader) { }
    }

    [Serializable]
    public class NewsletterUpdateResponse : ServerResponse
    {
        public static readonly string StaticHeader = "NewsletterSend";
        public int Number { get; set; }

        public NewsletterUpdateResponse() : base(StaticHeader) { }
    }
}
