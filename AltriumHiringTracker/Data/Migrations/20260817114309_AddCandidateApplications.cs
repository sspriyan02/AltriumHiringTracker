using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltriumHiringTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddCandidateApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidateApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VacancyId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HighestQualification = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    CvOriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CvStoredFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CvFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MatchCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatchedCriteria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MissingCriteria = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateApplications_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateApplications_VacancyId_Email",
                table: "CandidateApplications",
                columns: new[] { "VacancyId", "Email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateApplications");
        }
    }
}
