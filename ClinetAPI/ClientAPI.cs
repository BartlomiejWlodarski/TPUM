

namespace ClinetAPI
{
    [Serializable]
    public abstract class ServerCommand
    {
        public string Header;

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
    public class SellBookCommand : ServerCommand
    {
        public static string StaticHeader = "SellBook";

        public int BookID;
        public string Username = "";

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

        public string Username;

        public GetUserCommand(string username) : base(StaticHeader)
        {
            Username = username;
        }
    }
    [Serializable]
    public struct BookDTO
    {
        public int Id;
        public string Title;
        public string Author;
        public decimal Price;
        public bool Recommended;
        public int Genre;

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
        public string Username;
        public decimal Balance;
        public BookDTO[] Books;

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

        public BookDTO[]? Books;

        public AllBooksUpdateResponse() : base(StaticHeader) { }
    }

    [Serializable]
    public class BookChangedResponse : ServerResponse
    {
        public static readonly string StaticHeader = "BookChanged";

        public BookDTO book;
        public int changeType;

        public BookChangedResponse() : base(StaticHeader) { }
        public BookChangedResponse(int change) : base(StaticHeader) { changeType = change; }
    }


    [Serializable]
    public class TransactionResultResponse : ServerResponse
    {
        public static readonly string StaticHeader = "TransactactionResult";

        public int BookID;
        public string Username;
        public int ResultCode; 
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

        public UserDTO User;

        public UserChangedResponse() : base(StaticHeader) { }
    }

}
