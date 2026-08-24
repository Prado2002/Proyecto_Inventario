using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStockBodegaLoteKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoBodega",
                table: "StockBodegaLote");

            migrationBuilder.DropColumn(
                name: "Lote",
                table: "StockBodegaLote");

            migrationBuilder.DropColumn(
                name: "NombreProducto",
                table: "StockBodegaLote");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "StockBodegaLote");

            migrationBuilder.DropColumn(
                name: "FechaEntrada",
                table: "Lotes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StockBodegaLote",
                newName: "StockBodegaLoteID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaVencimiento",
                table: "Lotes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockBodegaLoteID",
                table: "StockBodegaLote",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "CodigoBodega",
                table: "StockBodegaLote",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lote",
                table: "StockBodegaLote",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreProducto",
                table: "StockBodegaLote",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "StockBodegaLote",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaVencimiento",
                table: "Lotes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEntrada",
                table: "Lotes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
