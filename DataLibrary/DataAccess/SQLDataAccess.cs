﻿using DataLibrary.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DataLibrary.DataAccess
{
    public static class SQLDataAccess
    {
        private static IConfiguration Configuration;

        public static void SetupConnection(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static List<GameModel> GetPurchasedGames(string Username)
        {
            List<GameModel> games = new List<GameModel>();

            return games;
        }

        public static List<PlayerModel> GetFriends(string Username)
        {
            List<PlayerModel> friends = new List<PlayerModel>();

            return friends;
        }

        public static List<StreamModel> GetFollowedStreams(string Username)
        {
            List<StreamModel> streams = new List<StreamModel>();

            return streams;
        }

        public static List<GameModel> GetWishlist(string Username)
        {
            List<GameModel> games = new List<GameModel>();

            return games;
        }

        public static string GetPlayerName(string Username)
        {
            string displayName = "";

            using (MySqlConnection conn = new MySqlConnection(Configuration["DBConn:ConnectionString"]))
            {
                conn.Open();
                Console.WriteLine("Opening Connection");
                MySqlCommand cmd = new MySqlCommand("get_DisplayName(@user_name)", conn);
                cmd.Parameters.AddWithValue("user_name", Username);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    displayName = rdr["PlayerDisplayName"].ToString();
                }
                conn.Close();
            }

            return displayName;
        }

        public static bool verifyPlayer(PlayerModel player)
        {
            bool playerExists = false;

            using (MySqlConnection conn = new MySqlConnection(Configuration["DBConn:ConnectionString"]))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("verify_Player", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", player.Username);
                cmd.Parameters.AddWithValue("@password", player.Password);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    playerExists = true;
                    player.DisplayName = rdr["PlayerDisplayName"].ToString();
                    player.Permission = Convert.ToInt32(rdr["PlayerPermission"]);
                }
                conn.Close();
            }

            return playerExists;
        }

        public static bool IsConnected()
        {
            bool connected = false;

            using (MySqlConnection conn = new MySqlConnection(Configuration["DBConn:ConnectionString"]))
            {
                conn.Open();
                connected = conn.Ping();
                conn.Close();
            }

            return connected;
        }
    }
}