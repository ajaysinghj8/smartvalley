import {Component, OnInit} from '@angular/core';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {AdminExpertsListItem} from './admin-experts-list-item';
import {LazyLoadEvent} from 'primeng/api';
import {PendingExpertListResponse} from '../../../api/expert/pending-expert-list-response';
import {AdminExpertItem} from './admin-expert-item';
import {AreaService} from '../../../services/expert/area.service';
import {AdminExpertResponse} from './admin-expert-response';
import {DialogService} from '../../../services/dialog-service';
import {ExpertContractClient} from '../../../services/contract-clients/expert-contract-client';
import {DeleteExpertRequest} from '../../../api/expert/delete-expert-request';
import {TranslateService} from '@ngx-translate/core';

@Component({
    selector: 'app-admin-experts-list',
    templateUrl: './admin-experts-list.component.html',
    styleUrls: ['./admin-experts-list.component.css']
})
export class AdminExpertsListComponent implements OnInit {
    private totalRecords: number;
    private loading: boolean;
    private cols: AdminExpertsListItem[];
    private currentPage = 0;
    private pageSize = 10;
    public expertsResponse: any;
    public experts: AdminExpertItem[] = [];
    public transactionHash: string;
    public deleteExpertRequest: DeleteExpertRequest;

    constructor(private expertApiClient: ExpertApiClient,
                private expertContractClient: ExpertContractClient,
                private dialogService: DialogService,
                private areaService: AreaService,
                private translateService: TranslateService) {
    }
    public async getExpertList(event: LazyLoadEvent) {
        this.currentPage = (event.first / event.rows);
        this.loading = true;
        this.expertsResponse = await this.expertApiClient.getExpertsListAsync(this.currentPage, this.pageSize);
        this.renderTableRows(this.expertsResponse.items);
        this.loading = false;

    }
    public renderTableRows (expertResponseItems: PendingExpertListResponse[]) {
        this.experts = [];
        for (const expert of expertResponseItems) {
            const expertItem = <AdminExpertItem>{
                name: expert.name,
                about: expert.about,
                address: expert.address,
                email: expert.email,
                isAvailable: expert.isAvailable,
                areas: this.areaService.getAreasByTypes(expert.areas),
            };
            this.experts.push(expertItem);
        }
    }
    public async showDialogToCreateNewExpert () {
        await this.dialogService.showCreateNewExpertModal();
    }

    async ngOnInit() {
        this.loading = true;
        this.expertsResponse = (await this.expertApiClient.getExpertsListAsync(this.currentPage, this.pageSize));
        this.totalRecords = this.expertsResponse.totalCount;
        this.renderTableRows(this.expertsResponse.items);

        this.loading = false;
        this.cols = [
            {field: 'address', header: this.translateService.instant('AdminExpertList.Address')},
            {field: 'email', header: this.translateService.instant('AdminExpertList.Email')},
            {field: 'name', header: this.translateService.instant('AdminExpertList.Name')},
            {field: 'about', header: this.translateService.instant('AdminExpertList.About')},
            {field: 'isAvailable', header: this.translateService.instant('AdminExpertList.Available')},
            {field: 'areas', header: this.translateService.instant('AdminExpertList.Categories')}
        ];
    }
    public async deleteExpertAsync (address, areas) {
        const areasId = this.areaService.getAreasIdByNames(areas);
        this.transactionHash = ( await this.expertContractClient.addSync(address, areasId) );
        this.deleteExpertRequest = {
            transactionHash: this.transactionHash,
            address: address
        };
        await this.expertApiClient.deleteExpertAsync(this.deleteExpertRequest);
    }
    public async showDialogToEditExpert (rowData: any) {
        await this.dialogService.showEditExpertModal(rowData);
    }
}
