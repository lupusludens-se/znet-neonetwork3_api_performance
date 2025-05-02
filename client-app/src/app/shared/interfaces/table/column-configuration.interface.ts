export interface ColumnConfigurationInterface {
  columnWidth?: string;
  name: string; // display name
  propertyName: string;
  imageProperty?: string;
  defaultImageUrl?: string;
  isBold?: boolean;
  isSortable?: boolean;
  sortName?: string;
  isCellContentClickable?: boolean;
  cellCssClass?: string; // root or other css defined in styles folder
}
