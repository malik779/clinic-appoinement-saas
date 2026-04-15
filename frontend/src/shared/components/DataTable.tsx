import { DataTable as PrimeDataTable } from "primereact/datatable";
import { Column, type ColumnProps } from "primereact/column";
import { InputText } from "primereact/inputtext";
import { useMemo, useState } from "react";

type DataTableProps<T extends object> = {
  rows: T[];
  columns: Array<ColumnProps & { field: keyof T & string; header: string }>;
  globalFilterFields: Array<keyof T & string>;
};

export function DataTable<T extends object>({
  rows,
  columns,
  globalFilterFields,
}: DataTableProps<T>) {
  const [search, setSearch] = useState("");

  const header = useMemo(
    () => (
      <div className="p-input-icon-left">
        <i className="pi pi-search" />
        <InputText
          value={search}
          onChange={(event) => setSearch(event.target.value)}
          placeholder="Search"
        />
      </div>
    ),
    [search],
  );

  return (
    <PrimeDataTable
      value={rows}
      paginator
      rows={10}
      rowsPerPageOptions={[10, 25, 50]}
      sortMode="multiple"
      removableSort
      globalFilter={search}
      globalFilterFields={globalFilterFields}
      header={header}
      stripedRows
      emptyMessage="No records found."
    >
      {columns.map((column) => (
        <Column key={column.field} {...column} />
      ))}
    </PrimeDataTable>
  );
}
