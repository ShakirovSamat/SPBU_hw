using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNUnitWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssembliesData",
                columns: table => new
                {
                    AssemblyDataId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AmountOfSucceedTests = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountOfFailTests = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountOfIgnoredTests = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountOfTest = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssembliesData", x => x.AssemblyDataId);
                });

            migrationBuilder.CreateTable(
                name: "MethodsData",
                columns: table => new
                {
                    MethodDataId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsSucceed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Time = table.Column<long>(type: "INTEGER", nullable: false),
                    IgnoreReason = table.Column<string>(type: "TEXT", nullable: true),
                    AssemblyDataId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodsData", x => x.MethodDataId);
                    table.ForeignKey(
                        name: "FK_MethodsData_AssembliesData_AssemblyDataId",
                        column: x => x.AssemblyDataId,
                        principalTable: "AssembliesData",
                        principalColumn: "AssemblyDataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MethodsData_AssemblyDataId",
                table: "MethodsData",
                column: "AssemblyDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MethodsData");

            migrationBuilder.DropTable(
                name: "AssembliesData");
        }
    }
}
