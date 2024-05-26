using QualityProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityProject.BL.Services
{
    public interface IFileGenerationService
    {
        Task<FileResult> GenerateCsvFileAsync(string csvContent);
        Task<FileResult> GeneratePdfFileAsync(string htmlContent);
        Task<FileResult> GenerateHtmlFileAsync(string htmlContent);
    }
}
