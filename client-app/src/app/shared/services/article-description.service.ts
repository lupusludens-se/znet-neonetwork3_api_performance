export class ArticleDescriptionService {
  static clearContent(description: string): string {
    return description
      .replace(/\[neo_pdf\]/g, '')
      .replace(/\[neo_video\]/g, '')
      .replace(/<p><\/p>/g, '');
  }
}
