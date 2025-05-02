import { Injectable } from '@angular/core';
import { ToolInterface } from '../../shared/interfaces/tool.interface';
import { ToolsApiEnum } from '../../shared/enums/api/tools-api.enum';
import { Observable } from 'rxjs';
import { HttpService } from '../../core/services/http.service';
import { Router } from '@angular/router';
import { CoreService } from '../../core/services/core.service';
import { PostInterface } from '../../+learn/interfaces/post.interface';
import { CATEGORIES, REGIONS, SOLUTIONS, TECHNOLOGIES } from '../../shared/constants/taxonomy-names.const';

@Injectable()
export class PinnedToolsRequestsService {
  constructor(
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly router: Router
  ) {}

  savePinnedTools(tools: ToolInterface[]): Observable<ToolInterface> {
    const pinnedTools = tools.filter(tool => tool.isPinned).map(tool => ({ toolId: tool.id }));
    return this.httpService.post(ToolsApiEnum.PinTools, pinnedTools);
  }

  fetchPinnedTools(): Observable<ToolInterface[]> {
    return this.httpService.get<ToolInterface[]>(ToolsApiEnum.PinTools, {
      filterBy: 'IsActive',
      expand: 'icon'
    });
  }

  pinTool(tools: ToolInterface[], selectedItems: ToolInterface[], index: number): void {
    if (tools[index].isPinned) {
      tools[index].isPinned = false;
    } else if (selectedItems.length < 5) {
      tools[index].isPinned = !tools[index].isPinned;
    }
  }

  redirectToPage(path: string): void {
    this.router.navigate([path]);
  }

  setTags(post: PostInterface): void {
    post.postTags = [
      ...this.coreService.getTaxonomyTag(post, CATEGORIES),
      ...this.coreService.getTaxonomyTag(post, SOLUTIONS),
      ...this.coreService.getTaxonomyTag(post, TECHNOLOGIES),
      ...this.coreService.getTaxonomyTag(post, REGIONS)
    ];
  }
}
