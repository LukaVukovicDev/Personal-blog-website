import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../models/paged-result.model';
import { PostListItem } from '../models/post.model';
import { PostService } from './post.service';

describe('PostService', () => {
  let service: PostService;
  let httpMock: HttpTestingController;
  const baseUrl = `${environment.apiUrl}/posts`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PostService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(PostService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getPosts should request the posts endpoint without params when query is empty', () => {
    const expected: PagedResult<PostListItem> = {
      items: [],
      page: 1,
      pageSize: 9,
      totalCount: 0,
      totalPages: 0,
      hasPrevious: false,
      hasNext: false,
    };

    service.getPosts().subscribe((result) => {
      expect(result).toEqual(expected);
    });

    const req = httpMock.expectOne((request) => request.url === baseUrl);
    expect(req.request.method).toBe('GET');
    expect(req.request.params.keys().length).toBe(0);
    req.flush(expected);
  });

  it('getPosts should append search, category, tag and pagination params', () => {
    service
      .getPosts({ search: 'xss', category: 'web', tag: 'owasp', page: 2, pageSize: 5 })
      .subscribe();

    const req = httpMock.expectOne(
      (request) => request.url === baseUrl && request.params.keys().length === 5,
    );
    expect(req.request.params.get('search')).toBe('xss');
    expect(req.request.params.get('category')).toBe('web');
    expect(req.request.params.get('tag')).toBe('owasp');
    expect(req.request.params.get('page')).toBe('2');
    expect(req.request.params.get('pageSize')).toBe('5');
    req.flush({
      items: [],
      page: 2,
      pageSize: 5,
      totalCount: 0,
      totalPages: 0,
      hasPrevious: true,
      hasNext: false,
    });
  });

  it('getBySlug should GET the post by slug', () => {
    service.getBySlug('moj-prvi-post').subscribe();

    const req = httpMock.expectOne(`${baseUrl}/moj-prvi-post`);
    expect(req.request.method).toBe('GET');
    req.flush({});
  });

  it('addComment should POST the comment body to the post comments endpoint', () => {
    const postId = 'abc-123';

    service.addComment(postId, { body: 'Odličan post!' }).subscribe();

    const req = httpMock.expectOne(`${baseUrl}/${postId}/comments`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ body: 'Odličan post!' });
    req.flush({});
  });

  it('delete should DELETE the post by id', () => {
    const postId = 'abc-123';

    service.delete(postId).subscribe();

    const req = httpMock.expectOne(`${baseUrl}/${postId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
