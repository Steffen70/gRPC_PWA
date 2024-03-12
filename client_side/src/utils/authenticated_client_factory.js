class AuthInterceptor {
    constructor(token) {
        this.token = token;
    }

    intercept(request, invoker) {
        const metadata = request.getMetadata();
        metadata['Authorization'] = `Bearer ${this.token}`;

        const call = invoker(request);

        if (call instanceof Promise) {
            return call.then(response => {
                return response;
            });
        }

        return call;
    }
}


export function createAuthenticatedClient(ClientConstructor, baseAddress, token) {
    const authInterceptor = new AuthInterceptor(token);

    const client = new ClientConstructor(baseAddress, null, {
        unaryInterceptors: [authInterceptor],
        streamInterceptors: [authInterceptor],
    });

    return client;
}
