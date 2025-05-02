import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ArticleDescriptionService } from 'src/app/shared/services/article-description.service';
import { SaveContentService } from '../../../../shared/services/save-content.service';

@Component({
  selector: 'neo-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss']
})
export class ContentComponent {
  @Input() size: 'wide' | 'standard' = 'wide';
  @Input() id: number;
  @Input() title: string;
  @Input() description: string;
  @Input() tags: { id: number; name: string; taxonomy: string }[];
  @Input() image: string;
  @Input() isSaved: boolean;

  @Output() postClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() tagClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() saveClick: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private readonly saveContentService: SaveContentService) {}

  save(): void {
    if (this.isSaved) {
      this.saveContentService.deleteArticle(this.id).subscribe(() => {
        this.isSaved = false;
        this.saveClick.emit(false);
      });
    } else {
      this.saveContentService.saveArticle(this.id).subscribe(() => {
        this.isSaved = true;
        this.saveClick.emit(true);
      });
    }
  }

  getDescription(description: string): string {
    const maxLength = 125;
    const modifiedDescription = ArticleDescriptionService.clearContent(description);
    return modifiedDescription.length > maxLength
      ? modifiedDescription.slice(0, maxLength) + '...'
      : modifiedDescription;
  }
}
