using Newtonsoft.Json;

namespace ConnectionAPI
{
    internal static class ServerStatics
    {
        public static readonly string GetBooksCommandHeader = "GetBooks";
        public static readonly string SellBookCommandHeader = "SellBook";
        public static readonly string GetUserCommandHeader = "GetUser";

        public static readonly string AllBooksUpdateResponseHeader = "AllBooksUpdate";
        public static readonly string BookChangedResponseHeader = "BookChanged";
        public static readonly string TransactionResultResponseHeader = "TransactactionResult";
        public static readonly string UserChangedResponseHeader = "UserChanged";
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public abstract partial class ServerCommand
    {
        [Newtonsoft.Json.JsonProperty("Header", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Header { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class GetBooksCommand : ServerCommand
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class SellBookCommand : ServerCommand
    {
        [Newtonsoft.Json.JsonProperty("BookID", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int BookID { get; set; }

        [Newtonsoft.Json.JsonProperty("Username", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Username { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class GetUserCommand : ServerCommand
    {
        [Newtonsoft.Json.JsonProperty("Username", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Username { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class BookDTO
    {
        [Newtonsoft.Json.JsonProperty("Id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("Title", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("Author", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Author { get; set; }

        [Newtonsoft.Json.JsonProperty("Price", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal Price { get; set; }

        [Newtonsoft.Json.JsonProperty("Recommended", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Recommended { get; set; }

        [Newtonsoft.Json.JsonProperty("Genre", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Genre { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public abstract partial class ServerResponse
    {
        [Newtonsoft.Json.JsonProperty("Header", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Header { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class AllBooksUpdateResponse : ServerResponse
    {
        [Newtonsoft.Json.JsonProperty("Books", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<BookDTO> Books { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class BookChangedResponse : ServerResponse
    {
        [Newtonsoft.Json.JsonProperty("book", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BookDTO Book { get; set; }

        [Newtonsoft.Json.JsonProperty("changeType", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ChangeType { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class TransactionResultResponse : ServerResponse
    {
        [Newtonsoft.Json.JsonProperty("BookID", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int BookID { get; set; }

        [Newtonsoft.Json.JsonProperty("Username", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Username { get; set; }

        [Newtonsoft.Json.JsonProperty("ResultCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ResultCode { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class UserDTO
    {
        [Newtonsoft.Json.JsonProperty("Username", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Username { get; set; }

        [Newtonsoft.Json.JsonProperty("Balance", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal Balance { get; set; }

        [Newtonsoft.Json.JsonProperty("Books", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<BookDTO> Books { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class UserChangedResponse : ServerResponse
    {
        [Newtonsoft.Json.JsonProperty("User", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public UserDTO User { get; set; }


    }
}
