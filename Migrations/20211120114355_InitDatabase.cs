using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Squabble.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSso = table.Column<bool>(type: "bit", nullable: false),
                    MicrosoftSsoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    ServerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.ServerID);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    SenderID = table.Column<int>(type: "int", nullable: false),
                    ReceiverID = table.Column<int>(type: "int", nullable: false),
                    FriendRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => new { x.SenderID, x.ReceiverID });
                    table.ForeignKey(
                        name: "FK_FriendRequests_Accounts_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequests_Accounts_SenderID",
                        column: x => x.SenderID,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FriendID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => new { x.UserID, x.FriendID });
                    table.ForeignKey(
                        name: "FK_Friendships_Accounts_FriendID",
                        column: x => x.FriendID,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendships_Accounts_UserID",
                        column: x => x.UserID,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KanbanItems",
                columns: table => new
                {
                    KanbanItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ListName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanItems", x => x.KanbanItemID);
                    table.ForeignKey(
                        name: "FK_KanbanItems_Accounts_UserID",
                        column: x => x.UserID,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    SecurityQuestionOne = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SecurityAnswerOne = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SecurityQuestionTwo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SecurityAnswerTwo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => new { x.Email, x.UserName });
                    table.ForeignKey(
                        name: "FK_Logins_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AzureChatThreadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.ChannelId);
                    table.ForeignKey(
                        name: "FK_Channels_Servers_ServerID",
                        column: x => x.ServerID,
                        principalTable: "Servers",
                        principalColumn: "ServerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServerAdmins",
                columns: table => new
                {
                    ServerID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerAdmins", x => new { x.UserId, x.ServerID });
                    table.ForeignKey(
                        name: "FK_ServerAdmins_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerAdmins_Servers_ServerID",
                        column: x => x.ServerID,
                        principalTable: "Servers",
                        principalColumn: "ServerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerMembers",
                columns: table => new
                {
                    ServerID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerMembers", x => new { x.UserID, x.ServerID });
                    table.ForeignKey(
                        name: "FK_ServerMembers_Accounts_UserID",
                        column: x => x.UserID,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerMembers_Servers_ServerID",
                        column: x => x.ServerID,
                        principalTable: "Servers",
                        principalColumn: "ServerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerOwners",
                columns: table => new
                {
                    ServerID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerOwners", x => new { x.UserId, x.ServerID });
                    table.ForeignKey(
                        name: "FK_ServerOwners_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerOwners_Servers_ServerID",
                        column: x => x.ServerID,
                        principalTable: "Servers",
                        principalColumn: "ServerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChannelMembers",
                columns: table => new
                {
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelMembers", x => new { x.UserID, x.ChannelId });
                    table.ForeignKey(
                        name: "FK_ChannelMembers_Accounts_UserID",
                        column: x => x.UserID,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelMembers_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TimePosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "Avatar", "CommunicationToken", "CommunicationUserId", "Email", "FirstName", "IsSso", "MicrosoftSsoId", "MiddleName", "Surname", "UserName" },
                values: new object[,]
                {
                    { 1, null, "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDA4LWQxMzgtNGJmZS05NTNhMGQwMDkyNzAiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg2MzAiLCJleHAiOjE2Mzc0OTUwMzAsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODYzMH0.cj5gu6mDllNcB-yDDy69DKLHLQsTR_8mjiwSMeObi5newn3OxBOtQywxRgVzUGrwmVGsShw-BAfbEszWd9qfSNmZEJWu7Tz9G9v2WEUXrilpWGnZLcGGYg2tkMTGVpT3ckj-yj8t5xGIKXD_aI0xQaMdX0T9_345LqlQLymr3T4qSmhioK8_hcAoHGgJX6_ijJG--FNi893_nLkiAJWtKy1E-T1KL-vHSWcwX5cKbwlm0kcC2Wd5EkUHAcTngHmdIehDhfDrnRo3lwmmLC6ulaI_5zE1haSbJjUKa0jJVoDjSWPxwba8rwCkFpEAMHEfXUCzi0ZO7t9mwqFn7vHxhg", "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd08-d138-4bfe-953a0d009270", "test1@test.com", "Test", false, null, "Ing", "Dummy", "Dragonborn" },
                    { 2, null, "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDA5LTcyZWItNGJmZS05NTNhMGQwMDkyN2EiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg2MzEiLCJleHAiOjE2Mzc0OTUwMzEsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODYzMX0.EANz8Sv5cpFVHZRxIFa996tETXGZpBkG2LcqMoH59-YKsTHIfWUwhbwwgHLVx9Lvuw_LL6wT0-UXkeULVT7aKdnvtjUGmCNn8o2whufFrzRc9S4BVBqcjMHLlhIahZiL78cWMOMYvl0Lsjl2U4KNMCDBrhhqprTfV07rT1zGMfrWAtJhpUmp26n0bsd_aQOKOKKH5RfrRDIlKsadPFbbEAjrFM7Hou7aBpMUkncd9I6oPexqu6W2rFrGB2kQUNNEpDgGBwUWqX95UzNLY3N7lwkPE7i1dRYfPecscJzv8BeQ3ApmOF4NXNvKtB7fAkwAXj3Rp19Gk_uJFH4aPcY3qQ", "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd09-72eb-4bfe-953a0d00927a", "test2@test.com", "Flim", false, null, "", "Flam", "Hayzeus" },
                    { 3, null, "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDA5LWJkMmQtNGJmZS05NTNhMGQwMDkyN2MiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg2MzIiLCJleHAiOjE2Mzc0OTUwMzIsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODYzMn0.KMCjTdvdEreFEwZ0qoGAL_cch4NpEdhLHHxYWur9EFgLcyKLNbkv3R5QEWQu7V3Qn5x8KPm-C_Tu00UgNU6VTOGLQOfLiZD8CgIhj1KYLGzj-c8BhT-FTzpOx3rwmPI_O4R5jtAXOOxowBYV6z3S6d12BfALOhI82odsQC6c8kVKGNHOTA0xMn10ApydkVxn9Z3VnQ1PmsIROSnKHlSxfAlcpbvDMtnOHxE9wBzCdoLC2b4aSEJSgIDU5DnTXLb6HPQZa-ZjheTcooI2czYiSHZJnkKwbJ2SYJ1Sh6kNXAcm8-k3dHndxv30g6CbldqMkZlltvpjtGdh_qSUlkOJKg", "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd09-bd2d-4bfe-953a0d00927c", "test3@test.com", "Bob", false, null, "The", "Builder", "Handyman" },
                    { 4, null, "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDBhLTA5ZjItNGJmZS05NTNhMGQwMDkyODAiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg2MzMiLCJleHAiOjE2Mzc0OTUwMzMsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODYzM30.k5JEO0her-bqFHEXonVVIx3oIBDv0XV-6d3gBL8pMqxIHLNTbsXGKVzFED9DX3NFl176av_hXztUZIPjr8yFrkJE7zDY2rsnVqcvmjun1oKzKbQe5ZKZPmdpwczVvJJCnyrIRUZ8xHNA2Bzv6QVSKbkYjWUuzivIZ7cp-w4GODNt9q9xyvIX0XPaQVdDH3UXrOHMKfRYA7K79qmK5giKV1T1SgCXHBmOztzxw8xkNOolIxaW_8oMV2jUgXacWOKFR-Bro7EXsXRFL3X7I6qCOcuaNlf7cXBxQ5p7h66_lfv4QeeGIV0xsMrxoh8v0h847SrP8_GSylcAcKuwsm6-qA", "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd0a-09f2-4bfe-953a0d009280", "bikinibottom1@test.com", "Spongebob", false, null, "", "Squarepants", "Spongebob" },
                    { 5, null, "eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOF8wMDAwMDAwZC1kZDBhLTQ2YmEtNGJmZS05NTNhMGQwMDkyODIiLCJzY3AiOjE3OTIsImNzaSI6IjE2Mzc0MDg2MzMiLCJleHAiOjE2Mzc0OTUwMzMsImFjc1Njb3BlIjoiY2hhdCx2b2lwIiwicmVzb3VyY2VJZCI6IjAyNWZiYzQ4LThlYTEtNGNiMi05NTljLTQyYjRkYzllOTcyOCIsImlhdCI6MTYzNzQwODYzM30.E4JMIAT8Y0twP3DiN3aLaDEno8BFqTkP38ZmvAojIelVyXSkz9C6Qf9C9OgyeieMcPOFbfFCqNVWbLtJZbOwzD-FNM07GiQ4y96XZvbrywf5Bz9xvUy_bz6UsdX84xEneTbALRE68bh96oxb42t7dfE4TwfImXY8sAb2MW0uRtCmV2Uak0iDmI2nBklASnjIRVQfU_wjIHcxtjMg9dEWcv7r1C9FrGrRKY50pLA_-A2m63cS_rbENOTP_0vRjd-GioXMBH4wASnhhr2L94W46NcWSuCti7P-b3biNk-QzYtI7DCsski72Dzd6EutxT70IWVMEZ0mo9RY3P1ZW5bM-g", "8:acs:025fbc48-8ea1-4cb2-959c-42b4dc9e9728_0000000d-dd0a-46ba-4bfe-953a0d009282", "bikinibottom2@test.com", "Squidward", false, null, "", "Tentacles", "Squidward" }
                });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "ChannelId", "AzureChatThreadId", "ChannelName", "ServerID" },
                values: new object[] { 2, "19:pKMdnHy719c4TKjy4yT6XcJT44de74zY48-7pcBLTdM1@thread.v2", "1-2", null });

            migrationBuilder.InsertData(
                table: "Servers",
                columns: new[] { "ServerID", "ServerName" },
                values: new object[] { 1, "Test server :)" });

            migrationBuilder.InsertData(
                table: "ChannelMembers",
                columns: new[] { "ChannelId", "UserID" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "ChannelId", "AzureChatThreadId", "ChannelName", "ServerID" },
                values: new object[] { 1, "19:nzF3dny0zetNYvXBzLS02XA37IZrxVs7rrxqBzTCePY1@thread.v2", "Test Channel", 1 });

            migrationBuilder.InsertData(
                table: "Friendships",
                columns: new[] { "FriendID", "UserID" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Email", "UserName", "AccountId", "PasswordHash", "SecurityAnswerOne", "SecurityAnswerTwo", "SecurityQuestionOne", "SecurityQuestionTwo" },
                values: new object[,]
                {
                    { "test1@test.com", "Dragonborn", 1, "H+6k8J5Fo7d2BXba6VWm4rje5wuJ8Zk5PpfaWPZvH3RE4ptnfV45r9gbaX681uZg", "HxkBxqAm2K2HBlrKejIYzjIrCLR2qaE4sq0fPY7IX4gqtvoHRxE6bWSkf0n1u7pi", "HQZMoBZ+y272ejgjvXseUIvbHJDqI3OCnXwMIvQg+E3VTbbSlek6X6JauXXvHPdS", "My first pet's name?", "My second pet's name?" },
                    { "test2@test.com", "Hayzeus", 2, "ldmqLFrWzdlKNUarpoPt3EdFhaVYrSOAFvZMV0298XwRkfls9g+YHxUmMcYwmTny", "Qko7Ag6dcBfPx4UW76vjO4Jycm4nl3ajFWPwW+KXOMfXNx6Mr3Padd8UI3QTKMfY", "rOB4AHPd0D3A1t2RMAkwWd6BiS5woXenrnCrCW53TASe2NcfJ1gUmJB3cXS5hLvZ", "My first pet's name?", "My second pet's name?" },
                    { "test3@test.com", "Handyman", 3, "33WMjxiTCSgL2NwWx8wUwABsNtpy/E0xFQvKG5itqUbOnwPoZtQBhlco2T21306b", "Z/KdgRd52EHntvgneWQAgvmPfOJnZHLxBouZlgrfSBzYTlH6cCtCDKlqsh7bXayV", "1aywehyrqTN0184kCj92PCOHypgnw6nZwEyC/Th9yQccKOBSBkyBGloGOWaaBaKK", "My first pet's name?", "My second pet's name?" },
                    { "bikinibottom1@test.com", "Spongebob", 4, "eshQ+gqiTpkc3gFdKuA31pU18VLYkZGxWrV6KOtHX6xvnuLZAK4Cw4ScEet9UsZp", "2EeMilgOZL+o/Lj4HN/JGidPX7NnexHt2HqAGBEQzrd8tJx5RooYcT+7+Oc8xhIB", "6RGkTpRcig/ldSRFfqIc7xOH+zgYSx5aHenuXiRKyyaXT6WxzHhYW7mZo3UJFuTe", "My first pet's name?", "My second pet's name?" },
                    { "bikinibottom2@test.com", "Squidward", 5, "ldXuQ+xZg/xpfBQcYg0jKDw5lqakye12f6hgk1qrZb4PwN8/i1mIS2F72//ducLd", "34u+IXuUbFIfJEQ0VlAtB6TrWyZnjZMjbLQsTtKLjvVpGcIxFun20JYAOptyCMYD", "Rxwan39CQ/fiN/AcHw3UxPO1NOEKa7snhPbl9OVwmPjSCNXUfr0AEdOi2Dk/N8As", "My first pet's name?", "My second pet's name?" }
                });

            migrationBuilder.InsertData(
                table: "ServerAdmins",
                columns: new[] { "ServerID", "UserId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "ServerMembers",
                columns: new[] { "ServerID", "UserID" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 }
                });

            migrationBuilder.InsertData(
                table: "ServerOwners",
                columns: new[] { "ServerID", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMembers_ChannelId",
                table: "ChannelMembers",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_ServerID",
                table: "Channels",
                column: "ServerID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverID",
                table: "FriendRequests",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FriendID",
                table: "Friendships",
                column: "FriendID");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanItems_UserID",
                table: "KanbanItems",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_AccountId",
                table: "Logins",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ChannelId",
                table: "Posts",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerAdmins_ServerID",
                table: "ServerAdmins",
                column: "ServerID");

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_ServerID",
                table: "ServerMembers",
                column: "ServerID");

            migrationBuilder.CreateIndex(
                name: "IX_ServerOwners_ServerID",
                table: "ServerOwners",
                column: "ServerID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelMembers");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "KanbanItems");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "ServerAdmins");

            migrationBuilder.DropTable(
                name: "ServerMembers");

            migrationBuilder.DropTable(
                name: "ServerOwners");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
