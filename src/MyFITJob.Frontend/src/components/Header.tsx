import { Badge } from './ui/badge'
import Avatar from 'boring-avatars';

<Avatar name="Maria Mitchell" />;
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
            <Avatar name="JROGER" className="h-8 w-8" />
            <span className="text-sm font-medium">JROGER</span>
          </div>
        </div>
      </div>
    </header>
  )
} 