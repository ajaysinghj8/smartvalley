import {EstimateCommentRequest} from './estimate-comment-request';
import {AreaType} from '../../scoring/area-type.enum';

export interface SaveEstimatesRequest {
  projectId: number;
  areaType: AreaType;
  estimateComments: Array<EstimateCommentRequest>;
  conclusion: string;
}
