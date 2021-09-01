using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestEase;

namespace WebApplication1.ApiService
{
    public interface IApiService
    {
        [Get("/api/mobile/search/results")]
        [Header("Authorization", "Bearer 6D4943ADCBAF0074CEA9067DFF2D2CF129320ACB06DA2215A9E4547697027B51")]
        Task<string> GetSearch(string searchType, int pageSize, int pageIndex);
    }
}
