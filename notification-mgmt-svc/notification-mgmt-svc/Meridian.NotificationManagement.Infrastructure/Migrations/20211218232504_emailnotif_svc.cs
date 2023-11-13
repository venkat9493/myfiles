using Microsoft.EntityFrameworkCore.Migrations;

namespace Meridian.NotificationManagement.Infrastructure.Migrations
{
    public partial class emailnotif_svc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                columns: table => new
                {
                    TemplateId = table.Column<string>(type: "text", nullable: false),
                    TemplateBody = table.Column<string>(type: "text", nullable: true),
                    TemplateSubject = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.TemplateId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplate");
        }
    }
}
