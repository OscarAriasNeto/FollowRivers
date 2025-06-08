using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FollowRivers.Migrations
{
    /// <inheritdoc />
    public partial class Vanessa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "RiverAddresses",
                columns: table => new
                {
                    RiverAddressId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Address = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    CanCauseFlood = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PersonId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiverAddresses", x => x.RiverAddressId);
                    table.ForeignKey(
                        name: "FK_RiverAddresses_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiverAddresses_PersonId",
                table: "RiverAddresses",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiverAddresses");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
