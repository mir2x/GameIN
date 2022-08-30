using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameIN.Controllers
{
    public class QueryGenerator
    {
        public static string PopularGames()
        {
			return @"select g.GameName, g.Description from Game g 
                     join Review ri 
                     on g.GameName = ri.GameName
                     where ri.Review = 'Very Positive' or ri.Review = 'Mostly Positive' or  ri.Review = 'Positive'
                     order by YEAR(Release) DESC , MONTH(Release) ";
		}
        public static string RecentGames()
        {
            return @"select GameName, Description from Game
                    WHERE Release < cast(GETDATE() as date) and Release > '2022-01-01' ";
        }

        public static string FreeGames()
        {
            return @"select GameName, Description from Game
                     where Price = 0.00 ";
        }

        public static string SearchGames()
        {
            return @"select g.GameName, c.Category from Game g
                        join Category c
                        on g.GameName = c.GameName
                        where g.GameName like @search_term";
        }

        public static string AllUser()
        {
            return @"select Name, Email, Password from [dbo].[User] where Role = 'User'";
        }

        public static string AddFavorite()
        {
            return @"insert into Favorite(GameName, Email) Values(@Name, @Email)";
        }

        public static string FutureGames()
        {
            return @"select GameName from Game WHERE Release > cast(GETDATE() as date)";
        }
    }
}