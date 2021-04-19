// This class is copied to api.ts when running codegen.
// eslint-disable-next-line @typescript-eslint/no-unused-vars
export class BaseClient {
  public async transformOptions(options: RequestInit): Promise<RequestInit> {
    const token = localStorage.getItem("token");
    const headerData = token
      ? {
          ...options.headers,
          Authorization: "Bearer " + token,
        }
      : {
          ...options.headers,
        };
    return Object.assign({}, options, {
      headers: headerData,
    });
  }
}
