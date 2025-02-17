﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClubSync.Api.Migrations.Identity
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clubsync_role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clubsync_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clubsync_user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    given_name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clubsync_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clubsync_role_claim",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clubsync_role_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_clubsync_role_claim_clubsync_role_role_id",
                        column: x => x.role_id,
                        principalTable: "clubsync_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clubsync_user_claim",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clubsync_user_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_clubsync_user_claim_clubsync_user_user_id",
                        column: x => x.user_id,
                        principalTable: "clubsync_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clubsync_user_login",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clubsync_user_login", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_clubsync_user_login_clubsync_user_user_id",
                        column: x => x.user_id,
                        principalTable: "clubsync_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clubsync_user_role",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clubsync_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_clubsync_user_role_clubsync_role_role_id",
                        column: x => x.role_id,
                        principalTable: "clubsync_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_clubsync_user_role_clubsync_user_user_id",
                        column: x => x.user_id,
                        principalTable: "clubsync_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clubsync_user_token",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clubsync_user_token", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_clubsync_user_token_clubsync_user_user_id",
                        column: x => x.user_id,
                        principalTable: "clubsync_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "clubsync_role",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clubsync_role_claim_role_id",
                table: "clubsync_role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "clubsync_user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "clubsync_user",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clubsync_user_claim_user_id",
                table: "clubsync_user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_clubsync_user_login_user_id",
                table: "clubsync_user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_clubsync_user_role_role_id",
                table: "clubsync_user_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clubsync_role_claim");

            migrationBuilder.DropTable(
                name: "clubsync_user_claim");

            migrationBuilder.DropTable(
                name: "clubsync_user_login");

            migrationBuilder.DropTable(
                name: "clubsync_user_role");

            migrationBuilder.DropTable(
                name: "clubsync_user_token");

            migrationBuilder.DropTable(
                name: "clubsync_role");

            migrationBuilder.DropTable(
                name: "clubsync_user");
        }
    }
}
