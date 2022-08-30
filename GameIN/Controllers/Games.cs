using GameIN.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace GameIN.Controllers
{
    public class Games
    {
        public static void AddGame(GameModel game)
        {
            MemoryStream ms = new MemoryStream();
            game.Poster.InputStream.CopyTo(ms);
            byte[] poster_buffer = ms.ToArray();

            MemoryStream ms2 = new MemoryStream();
            game.GamePlay.InputStream.CopyTo(ms2);
            byte[] gameplay_buffer = ms.ToArray();

            string query = @"insert into Game(GameName, Description, Release, Price) Values(@GameName, @Description, @Release, @Price) insert into Category(GameName, Category) Values(@GameName, @Category) insert into Poster(GameName, Image) Values(@GameName, @Poster) insert into GamePlay(GameName, Image) Values(@GameName, @GamePlay) insert into Developer(GameName, Developer) Values(@GameName, @Developer) insert into Publisher(GameName, Publisher) Values(@GameName, @Publisher) insert into Platform(GameName, Platform) Values(@GameName, @Platform) insert into Processor(GameName, Processor) Values(@GameName, @Processor) insert into Ram(GameName, Ram) Values(@GameName, @Ram) insert into Graphics(GameName, Graphics) Values(@GameName, @Graphics) insert into Storage(GameName, Storage) Values(@GameName, @Storage) insert into Directx(GameName, Directx) Values(@GameName, @Directx) insert into Review(GameName, Review) Values(@GameName, @Review)";
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@GameName", game.Name);
            command.Parameters.AddWithValue("@Description", game.Description);
            command.Parameters.AddWithValue("@Release", game.Release);
            command.Parameters.AddWithValue("@Price", game.Price);
            command.Parameters.AddWithValue("@Category", game.Category);
            command.Parameters.AddWithValue("@Poster", poster_buffer);
            command.Parameters.AddWithValue("@GamePlay", gameplay_buffer);
            command.Parameters.AddWithValue("@Developer", game.Developer);
            command.Parameters.AddWithValue("@Publisher", game.Publisher);
            command.Parameters.AddWithValue("@Platform", game.Platform);
            command.Parameters.AddWithValue("@Processor", game.Processor);
            command.Parameters.AddWithValue("@Ram", game.Ram);
            command.Parameters.AddWithValue("@Graphics", game.Graphics);
            command.Parameters.AddWithValue("@Storage", game.Storage);
            command.Parameters.AddWithValue("@Directx", game.Directx);
            command.Parameters.AddWithValue("@Review", game.Review);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }
}