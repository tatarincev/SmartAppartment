namespace Assessment2.Models
{
    /// <summary>
    /// Represents all the options you can use to configure OAuth authorization flows.
    /// </summary> 
    public class AuthOptions
    {
        /// <summary>
        /// Authority (url to authorization server)
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        ///  Audience refers to the Resource Servers that should accept the token
        /// </summary>
        public string ApiIdentifier { get; set; }  
    }
}
