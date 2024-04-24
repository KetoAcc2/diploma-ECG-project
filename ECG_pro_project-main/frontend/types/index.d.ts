declare module "*.jpg";
declare module "*.png";
declare module "*.docx";
declare module "*.bib" {
  const path: string;
  export default path;
}
