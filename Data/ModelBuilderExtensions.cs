using System.Collections.Generic;
using Azure.Communication;
using Azure.Communication.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;
using Squabble.Models;
using Squabble.Models.Entities;

namespace Squabble.Data
{
    public static class ModelBuilderExtensions
    {

        public static void Seed(this ModelBuilder modelBuilder)
        {
            var client = new CommunicationIdentityClient("endpoint=https://squabble-communication-service.communication.azure.com/;accesskey=i+rjCVXosX100n2EJh4pB+NrR8b4zdas4zCDUnjUcINFlW5YJjpOlfxACsOqqpDYct91FXG8ZhilIM6ctEbwsg==");
            //LoadIdentityTables(modelBuilder);

            LoadUsersAsync(modelBuilder, client);
            LoadLogins(modelBuilder);
            LoadGroupAdmins(modelBuilder);

            LoadGroups(modelBuilder);

            LoadGroupChannels(modelBuilder);

            LoadPosts(modelBuilder);

            LoadFriends(modelBuilder);

            //LoadAvatars(modelBuilder);

        }

        //TODO:
        public static void LoadAvatars(ModelBuilder modelBuilder)
        {

        }

        public static void LoadFriends(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>().HasData(
                new Friendship
                {
                    UserID = 1,
                    FriendID = 2
                },
                new Friendship
                {
                    UserID = 2,
                    FriendID = 1
                }
            );

            // One-to-one conversation channel.
            modelBuilder.Entity<Channel>().HasData(
                new Channel
                {
                    ChannelId = 2,
                    ChannelName = "1-2",
                    AzureChatThreadId = "19:pKMdnHy719c4TKjy4yT6XcJT44de74zY48-7pcBLTdM1@thread.v2"
                }
            );

            modelBuilder.Entity<ChannelMember>().HasData(
                new ChannelMember
                {
                    ChannelId = 2,
                    UserID = 1
                },
                new ChannelMember
                {
                    ChannelId = 2,
                    UserID = 2
                }
            );
        }

        public static void LoadUsersAsync(ModelBuilder modelBuilder, CommunicationIdentityClient _client)
        {
            var users = new User[] {
                new User {
                    UserName = "Dragonborn",
                    AccountId = 1,
                    FirstName = "Test",
                    MiddleName = "Ing",
                    Surname = "Dummy",
                    Email = "test1@test.com",
                    CommunicationToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDA4LWQxMzgtNGJmZS05NTNhMGQwMDkyNzAiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg1MTciLCJleHAiOjE2Mzc0OTQ5MTcsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODUxN30.f7EeXmi_EEQU9jDdyOu0HH7ufVi0m9XDARhc3B4tnvvVmPrrSY-b2xQo3N5FTk1VawAQePsX_hlPNQBp8xz9jZMnAQ2LXOr_VoIAHpWWcKXA0yoAW5Tx5uCWPOgIIgteO50ypqORjlsTPU3pw3JjbknvppuXmED0d6GzhdlGKZyNP71HPLb49ZMVzhKm4q7SPn2JF6YpS_YZmkrK_dZuL9IoRxljKYEqEIFxIIzemHfhEhAe05tJFTHm3XRA8em4Id0L0si9DA9KJgwydMsWkKSdlV_k4B6EiRaqAf_wL0mRb9TVHZhmi79RWkHusVlEr8bls-GmHvo5GEviFoRqKQ",
                    CommunicationUserId = "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd08-d138-4bfe-953a0d009270"
                },
                new User {
                    UserName = "Hayzeus",
                    AccountId = 2,
                    FirstName = "Flim",
                    MiddleName = "",
                    Surname = "Flam",
                    Email = "test2@test.com",
                    CommunicationToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDA5LTcyZWItNGJmZS05NTNhMGQwMDkyN2EiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg1NTgiLCJleHAiOjE2Mzc0OTQ5NTgsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODU1OH0.jf2wXDs5i3d145hcl67SjtmHumukeRdFbm7xK3gnBpIPSllU1dCIvac6q_oYqWXB50uwRmuoiCpogPOMZ12W0c7XSCmYCn9pJb2AXW6qdvJP0GJuYcHXLgWu8ox7-SlwWKO5_xWHXvG6532MZ9PGYDyqvPiWwjxuw13S-KRCKM6LiwPFwX6nX-3InUhMGXwJeJ7spSe87JZkXYY17WbEOSYU24THBOJeImjzSlTxLKgz12M-HblPXfI7Ol0h8nk0lNo7If9nXrlxCSFM2z1V3pOLcELBVPa0MbxdgFExmPhub5LOxfVrBHmaekDW6OvdRX_ZvETJ-Y5g2HQtz6qhiQ",
                    CommunicationUserId = "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd09-72eb-4bfe-953a0d00927a"
                },
                new User
                {
                    UserName = "Handyman",
                    AccountId = 3,
                    FirstName = "Bob",
                    MiddleName = "The",
                    Surname = "Builder",
                    Email = "test3@test.com",
                    CommunicationToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDA5LWJkMmQtNGJmZS05NTNhMGQwMDkyN2MiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg1NzciLCJleHAiOjE2Mzc0OTQ5NzcsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODU3N30.UVknT3QJSZffAV5KuQML93XP4YeuWp0YOYQPoDPNmJZ_qvlH6CJ-STYXnbJ7XePOxzUPrKpve3ZYvSBGy5bB9noc32n9w5rI1UJXEXtLJ7UZwnbOSm08zzekOD9iYS4iSA2JSE2qA6qnf_FRZNjJFR013T7_RivR9Cj1GZ1BFSXA8M67qs4DEgCvFYiLYTkc8eIYRYtULz65_951jsY24vHlY_MZSRav2ryE0mwiU4sVgmWzIxUiYNe9wDIIsI_ih_EiJyYqhZVS4h2mpnc7NVqD4h-f-HYJSJvdoO9M0e90t1HsP9NMv1L5cuH4xek7u59ZOWKhaQhYcX9MuBkQqw",
                    CommunicationUserId = "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd09-bd2d-4bfe-953a0d00927c"
                }
                ,
                new User
                {
                    UserName = "Spongebob",
                    AccountId = 4,
                    FirstName = "Spongebob",
                    MiddleName = "",
                    Surname = "Squarepants",
                    Email = "bikinibottom1@test.com",
                    CommunicationToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDBhLTA5ZjItNGJmZS05NTNhMGQwMDkyODAiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg1OTciLCJleHAiOjE2Mzc0OTQ5OTcsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODU5N30.sqeQddkZ51dwvg32Hvq26qq7yZV_9DSCBRHZP-TAksiuqLzMWKzKF3QrjC6LlHEUbSazhy6EeMroo9DLMtDTCf-gMu04eBs6IafkLZtVL2jlky7savuEejwU0NCYTFgFvDnZ3rK2s9tRynLArH9ti1sT-AAUS6_jvF4xIo9b_X1L9fx5cf0LJgLLR78JHiON9DXbtEd85rWWfNbnv6vQ6m_8pdQ4BQBbThze97zw5t-MAy-C0BKakpSEwcF4UM46AKuPTj747qZZwTT0koagozlWxBGox5bjyVtW1F2NP9ddtSMmF1apyXLbLKQlD2J5XpYidSiiTUyIXhii4b_X4g",
                    CommunicationUserId = "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd0a-09f2-4bfe-953a0d009280"
                },
                new User
                {
                    UserName = "Squidward",
                    AccountId = 5,
                    FirstName = "Squidward",
                    MiddleName = "",
                    Surname = "Tentacles",
                    Email = "bikinibottom2@test.com",
                    CommunicationToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDBhLTQ2YmEtNGJmZS05NTNhMGQwMDkyODIiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg2MTMiLCJleHAiOjE2Mzc0OTUwMTMsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODYxM30.XSLdbW6TuxefaV_8DrUomSDIDHNqMkb7fXPgqbHb8UdGFLMQIKSx6pOAJlF0IMR_Dsbs4VUxV85ZqGKsgEhhq3UH99pl-S76LZit1Geme4sf_AJvjknp2NTiOef6Bqs-coPXVx17zvSaskJIGpgt7XfHWg1fP-givcpC_B4BBkQMeMWg8gWuMOgPXEHOlXMe2AEbiaxARHYedXA5fNKa5EEtrITG1ftXQoyKaXuiWiF89OGzAkHDWXzC2cZ22IEFT_k8ed2foCLgb2xEDoLbg1V5MWUhQ6RAUIJd3Cjir-48H2CZJg3LFluf9Z5y1vf4QwEqh5HBqaZe5avqxp38DA",
                    CommunicationUserId = "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd0a-46ba-4bfe-953a0d009282"
                }
            };



            foreach (var user in users)
            {
                IEnumerable<CommunicationTokenScope> scope = new[] { CommunicationTokenScope.Chat, CommunicationTokenScope.VoIP };
                var identifier = new CommunicationUserIdentifier(user.CommunicationUserId);
                var response = _client.GetTokenAsync(identifier, scope).Result;
                var responseValue = response.Value;
                user.CommunicationToken = responseValue.Token;
            }

            modelBuilder.Entity<User>().HasData(users);



        }

        public static void LoadLogins(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Login>().HasData(


                new Login
                {
                    Email = "bikinibottom1@test.com",
                    UserName = "Spongebob",
                    PasswordHash = SimpleHashing.PBKDF2.Hash("abc123"),
                    AccountId = 4,
                    SecurityQuestionOne = "My first pet's name?",
                    SecurityAnswerOne = PBKDF2.Hash("Bruce"),
                    SecurityQuestionTwo = "My second pet's name?",
                    SecurityAnswerTwo = PBKDF2.Hash("Charlotte")
                },
                new Login
                {
                    Email = "bikinibottom2@test.com",
                    UserName = "Squidward",
                    PasswordHash = SimpleHashing.PBKDF2.Hash("abc123"),
                    AccountId = 5,
                    SecurityQuestionOne = "My first pet's name?",
                    SecurityAnswerOne = PBKDF2.Hash("Bruce"),
                    SecurityQuestionTwo = "My second pet's name?",
                    SecurityAnswerTwo = PBKDF2.Hash("Charlotte")

                },




                new Login
                {
                    Email = "test1@test.com",
                    UserName = "Dragonborn",
                    PasswordHash = PBKDF2.Hash("abc123"),
                    AccountId = 1,
                    SecurityQuestionOne = "My first pet's name?",
                    SecurityAnswerOne = PBKDF2.Hash("Bruce"),
                    SecurityQuestionTwo = "My second pet's name?",
                    SecurityAnswerTwo = PBKDF2.Hash("Charlotte")
                },
                new Login
                {
                    Email = "test2@test.com",
                    UserName = "Hayzeus",
                    PasswordHash = PBKDF2.Hash("dial911"),
                    AccountId = 2,
                    SecurityQuestionOne = "My first pet's name?",
                    SecurityAnswerOne = PBKDF2.Hash("Bruce"),
                    SecurityQuestionTwo = "My second pet's name?",
                    SecurityAnswerTwo = PBKDF2.Hash("Charlotte")
                },
                new Login
                {
                    Email = "test3@test.com",
                    UserName = "Handyman",
                    PasswordHash = PBKDF2.Hash("ABC123"),
                    AccountId = 3,
                    SecurityQuestionOne = "My first pet's name?",
                    SecurityAnswerOne = PBKDF2.Hash("Bruce"),
                    SecurityQuestionTwo = "My second pet's name?",
                    SecurityAnswerTwo = PBKDF2.Hash("Charlotte")
                }
            );

        }

        public static void LoadGroups(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Server>().HasData(
                new Server
                {
                    ServerID = 1,
                    ServerName = "Test server :)",
                    Channels = new List<Channel>(),

                }

            );
        }
        public static void LoadGroupAdmins(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerOwner>().HasData(
                new ServerOwner
                {
                    ServerID = 1,
                    UserId = 1
                });
            modelBuilder.Entity<ServerAdmin>().HasData(
                new ServerAdmin
                {
                    ServerID = 1,
                    UserId = 2
                }

            );
            modelBuilder.Entity<ServerMember>().HasData(
                new ServerMember
                {
                    ServerID = 1,
                    UserID = 3
                },
                new ServerMember
                {
                    ServerID = 1,
                    UserID = 4
                },
                new ServerMember
                {
                    ServerID = 1,
                    UserID = 5
                }
            );

        }


        public static void LoadGroupChannels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>().HasData(
                new Channel
                {
                    ChannelId = 1,
                    ServerID = 1,
                    ChannelName = "Test Channel",
                    AzureChatThreadId = "19:nzF3dny0zetNYvXBzLS02XA37IZrxVs7rrxqBzTCePY1@thread.v2"
                }
            );
        }

        public static void LoadPosts(ModelBuilder modelBuilder)
        {
       //     modelBuilder.Entity<Post>().HasData(

           // );

        }



    }

}
