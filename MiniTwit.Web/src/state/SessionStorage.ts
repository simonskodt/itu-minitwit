export function checkLogIn(): boolean {
    const isLoggedIn = sessionStorage.getItem('isLoggedIn')
    if (isLoggedIn === 'true') {
        return true
    } else {
        return false
    }
}

export function getCurrentUsername(): string {
    const username = sessionStorage.getItem('username') 
    
    if (username === null) {
        return ''
    }

    return username
}
