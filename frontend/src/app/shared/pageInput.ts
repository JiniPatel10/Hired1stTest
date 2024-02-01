export class PageInput {
    first?: number = 0;
    rows?: number = 25;
    sortField?: string = 'id';
    sortOrder?: number = 0;
    filters?: {
      [s: string]: any;
    };
    page?: number = 0;
}