using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Axinom.ControlPanel.Controllers
{
    public class UploadModel : IRequest<HttpResponseMessage>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [IsZipFile]
        public IFormFile File { get; set; }
    }

    public class IsZipFile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                Console.WriteLine(file);
                if (file.FileName.EndsWith(".zip"))
                    return ValidationResult.Success;
               
            }
            return new ValidationResult("Files must be in .zip format");
        }
    }
}