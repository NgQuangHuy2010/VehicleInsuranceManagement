using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project3.Models;

public partial class ContactUs
{
    public int Id { get; set; }
    //[Required(ErrorMessage = "FullName is required")]
    public string? FullName { get; set; }
    //[Required(ErrorMessage = "Email is required")]
    //[EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }
   // [Required(ErrorMessage = "Topic is required")]
    public string? Topic { get; set; }
   // [Required(ErrorMessage = "Phone is required")]
    public string? Phone { get; set; }
    //[Required(ErrorMessage = "Message is required")]
    public string? Message { get; set; }

    public bool? IsReplied { get; set; }
   // [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }
 // [Required(ErrorMessage = "Body is required")]
   
    public string ?Body { get; set; }
}
