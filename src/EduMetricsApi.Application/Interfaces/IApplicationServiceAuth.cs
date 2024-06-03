using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduMetricsApi.Application.Interfaces;

public interface IApplicationServiceAuth
{
    public string Authenticate(string username, string password);
}