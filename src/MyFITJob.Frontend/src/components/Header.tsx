import { Avatar, AvatarFallback, AvatarImage } from './ui/avatar'
import { Badge } from './ui/badge'

export function Header() {
  return (
    <header className="border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
      <div className="container flex h-16 items-center justify-between px-4">
        <div className="flex items-center space-x-4">
          <h1 className="text-xl font-bold">MyFITJob</h1>
          <Badge variant="secondary">TD CI/CD</Badge>
        </div>
        
        <div className="flex items-center space-x-4">
          <div className="flex items-center space-x-2">
            <Avatar className="h-8 w-8">
              <AvatarImage src="/avatar-placeholder.png" alt="Avatar" />
              <AvatarFallback>TODO</AvatarFallback>
            </Avatar>
            <span className="text-sm font-medium">TODO</span>
          </div>
        </div>
      </div>
    </header>
  )
} 