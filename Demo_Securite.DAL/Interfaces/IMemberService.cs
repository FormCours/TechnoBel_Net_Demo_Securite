using Demo_Securite.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Securite.DAL.Interfaces
{
    public interface IMemberService: ICrudService<int, Member>
    {
        // Pour ajouter des méthodes (hors Crud)
    }
}
