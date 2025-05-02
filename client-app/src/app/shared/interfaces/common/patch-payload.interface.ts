export interface PatchPayloadInterface {
  jsonPatchDocument: [
    {
      op: string;
      value: string | number;
      path: string;
    }
  ];
}
