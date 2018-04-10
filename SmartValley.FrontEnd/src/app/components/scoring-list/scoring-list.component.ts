import {Component, OnInit} from '@angular/core';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {SortDirection} from '../../api/sort-direction.enum';
import {SelectItem} from 'primeng/api';
import {DictionariesService} from '../../services/common/dictionaries.service';
import {OffersOrderBy} from '../../api/scoring-offer/offers-order-by';
import {ScoringOfferResponse} from '../../api/scoring-offer/scoring-offer-response';
import {OffersApiClient} from '../../api/scoring-offer/offers-api-client';
import {OfferStatus} from '../../api/scoring-offer/offer-status.enum';
import {OffersQuery} from '../../api/scoring-offer/offers-query';

@Component({
  selector: 'app-scoring-list',
  templateUrl: './scoring-list.component.html',
  styleUrls: ['./scoring-list.component.css']
})
export class ScoringListComponent implements OnInit {

  public ASC: SortDirection = SortDirection.Ascending;
  public DESC: SortDirection = SortDirection.Descending;
  public offers: ScoringOfferResponse[] = [];
  public checkOffers: boolean;
  public sortedBy: OffersOrderBy;
  public sortDirection: SortDirection;
  public offersOnPageCount = 10;
  public totalOffers: number;
  public offerStatuses: SelectItem[];
  public selectedOfferStatus: OfferStatus = null;
  public defaultStatus = <SelectItem>{
    label: 'All',
    value: null
  };

  public OfferStatus = OfferStatus;

  constructor(private router: Router,
              private dictionariesService: DictionariesService,
              private offersApiClient: OffersApiClient) {
    this.offerStatuses = this.dictionariesService.offerStatuses.map(i => <SelectItem>{
      label: i.value,
      value: i.id
    });
    this.offerStatuses.unshift(this.defaultStatus);
  }

  async ngOnInit() {
    this.sortDirection = this.DESC;
    this.sortedBy = OffersOrderBy.Status;

    await this.updateOffersAsync(0);
  }

  public async selectedStatusAsync(statusId: OfferStatus): Promise<void> {
    this.selectedOfferStatus = statusId;
    await this.updateOffersAsync(0);
  }

  public navigateToProject(id) {
    this.router.navigate([Paths.Project + '/' + id]);
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }

  public async updateOffersAsync(page: number) {
    const offersResponse = await this.offersApiClient.queryAsync(<OffersQuery>{
      offset: page * this.offersOnPageCount,
      count: this.offersOnPageCount,
      status: this.selectedOfferStatus,
      orderBy: this.sortedBy,
      sortDirection: this.sortDirection
    });
    this.offers = offersResponse.items;
    this.totalOffers = offersResponse.totalCount;
    this.checkOffers = true;
  }

  public async changePage(event) {
    await this.updateOffersAsync(event.page);
  }

  public async sortByName() {
    return this.sortBy(OffersOrderBy.Name);
  }

  public async sortByDeadline() {
    return this.sortBy(OffersOrderBy.Deadline);
  }

  public async sortByStatus() {
    return this.sortBy(OffersOrderBy.Status);
  }

  public async sortBy(orderBy: OffersOrderBy): Promise<void> {
    if (this.sortedBy === orderBy) {
      if (this.sortDirection === SortDirection.Descending) {
        this.sortDirection = SortDirection.Ascending;
      } else {
        this.sortDirection = SortDirection.Descending;
      }
    }
    this.sortedBy = orderBy;
    await this.updateOffersAsync(0);
  }

  navigateToProjectScoring(projectId: number, area: number, event: MouseEvent) {
    event.stopPropagation();
    this.router.navigate([Paths.ScoringOffer + '/' + projectId + '/' + area]);
  }

  navigateToEstimateScoring(projectId: number, area: number) {
    this.router.navigate([Paths.Project + '/' + projectId + '/scoring']);
  }

  public isSortableField(sortField: string): boolean {
    return this.sortedBy === OffersOrderBy[sortField];
  }

  public isSortableDirection(direction: SortDirection): boolean {
    return this.sortDirection === direction;
  }
}